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
using Transmitly.Infobip;
using Transmitly.ChannelProvider.Infobip.Sms.SendSmsMessage;
using System.Text;

namespace Transmitly.ChannelProvider.Infobip.Sms
{
	internal sealed class SmsChannelProviderClient(InfobipChannelProviderConfiguration configuration) : ChannelProviderRestClient<ISms>(null)
	{
		private const string SendAdvancedSmsMessage = "sms/2/text/advanced";
		private readonly InfobipChannelProviderConfiguration _configuration = configuration;

		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

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
						CreateSingleMessageRequestContent(recipient, communication),
						cancellationToken
					)
					.ConfigureAwait(false);

				var responseContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

				if (!result.IsSuccessStatusCode)
				{
					var error = JsonSerializer.Deserialize<ApiRequestErrorResult>(responseContent);
					results.Add(new InfobipDispatchResult
					{
						DispatchStatus = DispatchStatus.Error,
						ResourceId = error?.ServiceException?.MessageId,
						Exception = new ApiResultException(error),
					});
					Error(communicationContext, communication);
				}
				else
				{
					var success = Guard.AgainstNull(JsonSerializer.Deserialize<SendSmsMessageResponse>(responseContent));
					foreach (var message in success.Messages)
					{

						results.Add(new InfobipDispatchResult
						{
							ResourceId = message.MessageId,
							DispatchStatus = message.Status.GroupName.ToDispatchStatus()
						});

						Dispatched(communicationContext, communication);
					}
				}
			}

			return results;
		}

		private HttpContent CreateSingleMessageRequestContent(IAudienceAddress recipient, ISms sms)
		{
			var smsProperties = new ExtendedSmsChannelProperties(sms.ExtendedProperties);

			Guard.AgainstNull(sms.From);

			var messages = new SendSmsMessageRequestMessage(sms.Body, [new SendSmsMessageRequestMessageDestination(recipient.Value)])
			{
				ValidityPeriod = smsProperties.ValidityPeriod,
				EntityId = smsProperties.EntityId,
				ApplicationId = smsProperties.ApplicationId
			};

			var request = new SendSmsMessageRequest([messages])
			{
				From = sms.From?.Value
			};

			return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
		}



		protected override void ConfigureHttpClient(HttpClient client)
		{
			RestClientConfiguration.Configure(client, _configuration);
			base.ConfigureHttpClient(client);
		}
	}
}