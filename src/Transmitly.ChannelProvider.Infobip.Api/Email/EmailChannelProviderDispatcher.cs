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
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System;
using Transmitly.Delivery;
using Transmitly.ChannelProvider.Infobip.Api.Sms.SendSmsMessage;
using Transmitly.ChannelProvider.Infobip.Configuration;
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Email
{

	public sealed class EmailChannelProviderDispatcher(InfobipChannelProviderConfiguration configuration) : ChannelProviderRestDispatcher<IEmail>(null)
	{
		private const string SendEmailPath = "email/3/send";
		private readonly InfobipChannelProviderConfiguration _configuration = configuration;

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
					await CreateMessageContent(recipientList, communication, communicationContext),
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
			return results;
		}

		private static async Task<HttpContent> CreateMessageContent(IPlatformIdentityAddress[] recipients, IEmail email, IDispatchCommunicationContext context)
		{
			MultipartFormDataContent form = [];
			var emailProperties = new EmailExtendedChannelProperties(email.ExtendedProperties);
			bool hasTemplateId = emailProperties.TemplateId.HasValue && emailProperties.TemplateId.Value > 0;
			var messageId = Guid.NewGuid().ToString("N");
			TryAddRecipients(recipients, email, form);
			AddStringContent(form, EmailField.TemplateId, emailProperties.TemplateId?.ToString());
			AddStringContent(form, EmailField.From, email.From?.ToEmailAddress(), !hasTemplateId);
			AddStringContent(form, EmailField.Subject, email.Subject, !hasTemplateId);
			AddStringContent(form, EmailField.TextBody, email.TextBody);
			AddStringContent(form, EmailField.HtmlBody, email.HtmlBody);
			await TryAddAmpContent(email, context, form, emailProperties);
			AddStringContent(form, EmailField.IntermediateReport, emailProperties.IntermediateReport?.ToString().ToLowerInvariant());
			AddStringContent(form, EmailField.NotifyUrl, await GetNotifyUrl(messageId, emailProperties, email, context));
			AddStringContent(form, EmailField.Track, emailProperties.Track.ToString().ToLowerInvariant());
			AddStringContent(form, EmailField.TrackClicks, emailProperties.TrackClicks?.ToString().ToLowerInvariant());
			AddStringContent(form, EmailField.trackOpens, emailProperties.TrackOpens?.ToString().ToLowerInvariant());
			AddStringContent(form, EmailField.MessageId, messageId);
			AddStringContent(form, EmailField.ApplicationId, emailProperties.ApplicationId);
			AddStringContent(form, EmailField.EntityId, emailProperties.EntityId);

			return form;
		}

		private static async Task TryAddAmpContent(IEmail email, IDispatchCommunicationContext context, MultipartFormDataContent form, EmailExtendedChannelProperties emailProperties)
		{
			var ampTemplate = emailProperties.AmpHtml.GetTemplateRegistration(context.CultureInfo, false);
			if (ampTemplate == null)
				return;
			if (string.IsNullOrWhiteSpace(email.HtmlBody))
				throw new InfobipException("HtmlBody is required when using AmpHtml");

			var ampContent = await context.TemplateEngine.RenderAsync(ampTemplate, context);
			AddStringContent(form, EmailField.AmpHtml, ampContent, false);
		}

		private static async Task<string?> GetNotifyUrl(string messageId, EmailExtendedChannelProperties emailProperties, IEmail email, IDispatchCommunicationContext context)
		{
			string? url;
			var urlResolver = emailProperties.NotifyUrlResolver ?? email.DeliveryReportCallbackUrlResolver;
			if (urlResolver != null)
				url = await urlResolver(context).ConfigureAwait(false);
			else
			{
				url = emailProperties.NotifyUrl;
				if (string.IsNullOrWhiteSpace(url))
					return null;
			}

			if (string.IsNullOrWhiteSpace(url))
				return null;

			return new Uri(url).AddPipelineContext(messageId, context.PipelineIntent, context.PipelineId, context.ChannelId, context.ChannelProviderId).ToString();
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
			form.Add(new StringContent(value), string.Format("\"{0}\"", key));
		}

		private static void TryAddRecipients(IPlatformIdentityAddress[] recipients, IEmail email, MultipartFormDataContent form)
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