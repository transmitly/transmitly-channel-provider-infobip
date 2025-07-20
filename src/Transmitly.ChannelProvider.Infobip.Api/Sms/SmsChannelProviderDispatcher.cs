// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
//  
//  Licensed under the Apache License, Version 2.0 (the "License")
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using Transmitly.Delivery;
using System;
using Transmitly.ChannelProvider.Infobip.Configuration;
using Transmitly.ChannelProvider.Infobip.Api.Sms.SendSmsMessage;
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Sms
{
	public sealed class SmsChannelProviderDispatcher(InfobipChannelProviderConfiguration configuration) : ChannelProviderRestDispatcher<ISms>(null)
	{
		private const string SendAdvancedSmsMessage = "sms/2/text/advanced";
		private readonly InfobipChannelProviderConfiguration _configuration = configuration;

		protected override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, ISms communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(communication);
			Guard.AgainstNull(communicationContext);

			var recipients = communication.To ?? [];

			var results = new List<IDispatchResult>(recipients.Length);

			foreach (var recipient in recipients)
			{
				var result = await restClient
					.PostAsync(
						SendAdvancedSmsMessage,
						await CreateSingleMessageRequestContent(recipient, communication, communicationContext).ConfigureAwait(false),
						cancellationToken
					)
					.ConfigureAwait(false);

				var responseContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

				if (!result.IsSuccessStatusCode)
				{
					var error = JsonSerializer.Deserialize<ApiRequestErrorResult>(responseContent);
					results.Add(new InfobipDispatchResult
					{
						Status = CommunicationsStatus.ServerError(InfobipConstant.Id, "Exception"),
						ResourceId = error?.ServiceException?.MessageId,
						Exception = new ApiResultException(error),
					});
					Error(communicationContext, communication, results);
				}
				else
				{
					var success = Guard.AgainstNull(JsonSerializer.Deserialize<SendSmsMessageResponse>(responseContent));
					foreach (var message in success.Messages)
					{

						results.Add(new InfobipDispatchResult
						{
							ResourceId = message.MessageId,
							Status = message.Status.GroupName.ToDispatchStatus()
						});

						Dispatched(communicationContext, communication, results);
					}
				}
			}

			return results;
		}


		private async Task<HttpContent> CreateSingleMessageRequestContent(IPlatformIdentityAddress recipient, ISms sms, IDispatchCommunicationContext communicationContext)
		{
			var smsProperties = new SmsExtendedChannelProperties(sms.ExtendedProperties);

			var messageId = Guid.NewGuid().ToString("N");
			var bulkId = Guid.NewGuid().ToString("N");

			var messages = new SendSmsMessageRequestMessage(sms.Message, [new SendSmsMessageRequestMessageDestination(recipient.Value, messageId)])
			{
				ValidityPeriod = smsProperties.ValidityPeriod,
				EntityId = smsProperties.EntityId,
				ApplicationId = smsProperties.ApplicationId,
				NotifyUrl = await GetNotifyUrl(messageId, smsProperties, sms, communicationContext).ConfigureAwait(false),
				From = sms.From?.Value,
			};

			var request = new SendSmsMessageRequest([messages]) { BulkId = bulkId, };

			return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
		}

		private static async Task<string?> GetNotifyUrl(string messageId, SmsExtendedChannelProperties voiceProperties, ISms sms, IDispatchCommunicationContext context)
		{
			string? url;
			var urlResolver = voiceProperties.NotifyUrlResolver ?? sms.DeliveryReportCallbackUrlResolver;
			if (urlResolver != null)
				url = await urlResolver(context).ConfigureAwait(false);
			else
			{

				url = voiceProperties.NotifyUrl;
				if (string.IsNullOrWhiteSpace(url))
					return null;
			}

			if (string.IsNullOrWhiteSpace(url))
				return null;

			return new Uri(url).AddPipelineContext(messageId, context.PipelineIntent, context.PipelineId, context.ChannelId, context.ChannelProviderId).ToString();
		}

		protected override void ConfigureHttpClient(HttpClient httpClient)
		{
			RestClientConfiguration.Configure(httpClient, _configuration);
			base.ConfigureHttpClient(httpClient);
		}
	}
}