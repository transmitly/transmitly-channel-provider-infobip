// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
//  
//  Licensed under the Apache License, Version 2.0 (the "License")
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0000000
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Transmitly.ChannelProvider.Infobip.Sms.SendSmsMessage;
using Transmitly.Infobip;

namespace Transmitly.ChannelProvider.Infobip.Email
{

	internal sealed class EmailChannelProviderClient(InfobipChannelProviderConfiguration configuration, HttpClient? httpClient) : ChannelProviderRestClient<IEmail>(httpClient)
	{
		private const string SendEmailPath = "email/3/send";
		private readonly InfobipChannelProviderConfiguration _configuration = configuration;

		public EmailChannelProviderClient(InfobipChannelProviderConfiguration configuration) : this(configuration, null)
		{
			System.Text.Json.JsonSerializer.Serialize(new { });
		}

		protected override void ConfigureHttpClient(HttpClient client)
		{
			RestClientConfiguration.Configure(client, _configuration);
			base.ConfigureHttpClient(client);
		}

		protected override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, IEmail communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(communication);
			Guard.AgainstNull(communicationContext);

			var recipientList = communication.To ?? [];

			var results = new List<IDispatchResult>(recipientList.Length);

			var result = await restClient
				.PostAsync(
					SendEmailPath,
					CreateMessageContent(recipientList, communication),
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
			return results;
		}

		private static HttpContent CreateMessageContent(IAudienceAddress[] recipients, IEmail email)
		{
			MultipartFormDataContent form = [];
			var emailProperties = new ExtendedEmailChannelProperties(email.ExtendedProperties);
			bool hasTemplateId = emailProperties.TemplateId.HasValue && emailProperties.TemplateId.Value > 0;

			TryAddRecipients(recipients, email, form);
			AddStringContent(form, EmailField.TemplateId, emailProperties.TemplateId?.ToString());
			AddStringContent(form, EmailField.From, email.From?.ToEmailAddress(), !hasTemplateId);
			AddStringContent(form, EmailField.Subject, email.Subject, !hasTemplateId);
			AddStringContent(form, EmailField.TextBody, email.TextBody);
			AddStringContent(form, EmailField.HtmlBody, email.HtmlBody);
			AddStringContent(form, EmailField.IntermediateReport, emailProperties.IntermediateReport?.ToString().ToLowerInvariant());
			AddStringContent(form, EmailField.NotifyUrl, emailProperties.NotifyUrl);
			AddStringContent(form, EmailField.Track, emailProperties.Track.ToString().ToLowerInvariant());

			return form;
		}

		private static void AddStringContent(MultipartFormDataContent form, string key, string? value, bool optional = true)
		{
			if (value == null)
			{
				if (optional)
					return;
				else
					throw new InfobipException($"Cannot add {key} value is null.");
			}
			form.Add(new StringContent(value), key);
		}

		private static void TryAddRecipients(IAudienceAddress[] recipients, IEmail email, MultipartFormDataContent form)
		{
			var ccs = email.Cc ?? [];
			var bccs = email.Bcc ?? [];

			if (ccs.Length + bccs.Length + recipients.Length > 1000)
			{
				throw new InfobipException("Recipient count exceeds max of 1000.");
			}

			foreach (var to in recipients)
			{
				form.Add(new StringContent(to.ToEmailAddress()), EmailField.To);
			}

			foreach (var cc in ccs)
			{
				form.Add(new StringContent(cc.ToEmailAddress()), EmailField.Cc);
			}

			foreach (var bcc in bccs)
			{
				form.Add(new StringContent(bcc.ToEmailAddress()), EmailField.Bcc);
			}
		}
	}
}