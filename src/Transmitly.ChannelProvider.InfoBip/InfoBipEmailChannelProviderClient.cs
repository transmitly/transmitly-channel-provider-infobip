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

using IB = Infobip.Api.Client;
using IBApi = Infobip.Api.Client.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transmitly.ChannelProvider;
using Transmitly.Exceptions;
using System.Net.Http;
using Infobip.Api.Client;

namespace Transmitly.Infobip
{
	abstract class BaseClientChannelProviderRestClient(HttpClient? httpClient)
	{
		private static HttpClient? _staticHttpClient;
		private readonly HttpClient? _instanceHttpClient = httpClient;
		private readonly static object _resourceLock = new();

		protected HttpClient GetHttpClient()
		{
			if (_instanceHttpClient != null)
				return _instanceHttpClient;

			if (_staticHttpClient == null)
			{
				lock (_resourceLock)
				{
					if (_staticHttpClient == null)
					{
#pragma warning disable S2696 // Instance members should not write to "static" fields
						_staticHttpClient = new HttpClient();
#pragma warning restore S2696 // Instance members should not write to "static" fields
						ConfigureHttpClient(_staticHttpClient);
					}
				}
			}
			return _staticHttpClient;
		}

		protected virtual void ConfigureHttpClient(HttpClient client)
		{
			Guard.AgainstNull(client);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}
	}

	abstract class ClientChannelProviderRestClient<TCommunication>(Configuration configuration, HttpClient? httpClient) : BaseClientChannelProviderRestClient(httpClient), IChannelProviderClient<TCommunication>
	{

		protected HttpClient HttpClient => GetHttpClient();

		public IReadOnlyCollection<string>? RegisteredEvents => throw new NotImplementedException();

		IReadOnlyCollection<string>? IChannelProviderClient.RegisteredEvents => throw new NotImplementedException();

		protected ClientChannelProviderRestClient(Configuration configuration) : this(configuration, null)
		{

		}

		protected abstract Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, TCommunication communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken);
		protected virtual Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, object communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			if (communication is not TCommunication)
				throw new NotSupportedException();
			return DispatchAsync(restClient, (TCommunication)communication, communicationContext, cancellationToken);
		}

		Task<IReadOnlyCollection<IDispatchResult?>> IChannelProviderClient<TCommunication>.DispatchAsync(TCommunication communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			return DispatchAsync(GetHttpClient(), communication, communicationContext, cancellationToken);
		}

		Task<IReadOnlyCollection<IDispatchResult?>> IChannelProviderClient.DispatchAsync(object communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			return DispatchAsync(GetHttpClient(), communication, communicationContext, cancellationToken);
		}
	}

	internal sealed class InfobipEmailChannelProviderRestClient(Configuration configuration, HttpClient? httpClient) : ClientChannelProviderRestClient<IEmail>(configuration, httpClient)
	{
		private const string SendEmailPath = "/email/3/send";

		public InfobipEmailChannelProviderRestClient(Configuration configuration) : this(configuration, null)
		{
			System.Text.Json.JsonSerializer.Serialize(new { });
		}

		protected override void ConfigureHttpClient(HttpClient client)
		{
			client.BaseAddress = new Uri(configuration.BasePath);
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(configuration.ApiKeyPrefix, configuration.ApiKey);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}

		protected override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, IEmail communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			var result = await restClient.PostAsync(SendEmailPath, new StringContent(""), cancellationToken);

			throw new NotImplementedException();
		}
	}

	internal sealed class InfobipEmailChannelProviderClient(IB.Configuration optionObj) : ChannelProviderClient<IEmail>
	{
		private readonly IB.Configuration _optionObj = Guard.AgainstNull(optionObj);

		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

		public override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(IEmail email, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(email);
			Guard.AgainstNull(communicationContext);
			var messageId = Guid.NewGuid().ToString("N");
			var apiClient = new IBApi.SendEmailApi(_optionObj);
			var to = email.To?.FirstOrDefault()?.ToEmailAddress();
			var cc = string.Join(";", email.Cc?.Concat(email.To?.Skip(1) ?? []) ?? []);
			var bcc = string.Join(";", email.Bcc?.ToList() ?? []);
			var attachment = email.Attachments.FirstOrDefault()?.ContentStream;
			var from = email.From.ToEmailAddress();
			var sendResult = await apiClient.SendEmailAsync(from, to, email.Subject, cc, bcc, email.TextBody, null, messageId, null, attachment, null, email.HtmlBody, from, cancellationToken: cancellationToken);
			if (sendResult.Messages.Count != 1)
			{
				Error(communicationContext, email);
				throw new CommunicationsException("Unexpected # of messages");
			}

			var message = sendResult.Messages[0];
			Dispatched(communicationContext, email);
			return [new InfobipDispatchResult
			{
				ResourceId = message.MessageId,
				DispatchStatus = DispatchStatus.Dispatched,
			}];
		}
	}
}