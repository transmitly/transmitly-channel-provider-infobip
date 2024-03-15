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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transmitly.ChannelProvider;
using System.Net.Http;
using Infobip.Api.Client;
using Newtonsoft.Json;
using System.Text;
using Transmitly.ChannelProvider.Infobip.Voice;
using System.Globalization;

namespace Transmitly.Infobip
{
	internal sealed class InfobipVoiceChannelProviderClient(IB.Configuration optionObj) : ChannelProviderClient<IVoice>
	{
		private const string SingleMessagePath = "tts/3/single";
		private HttpClient? _httpClient;
		private static readonly object _resourceLock = new();
		private readonly Configuration _optionObj = Guard.AgainstNull(optionObj);

		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

		public override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(IVoice voice, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(voice);
			Guard.AgainstNull(communicationContext);

			var recipients = voice.To ?? [];

			var results = new List<IDispatchResult>(recipients.Length);

			foreach (var recipient in recipients)
			{
				var result = await GetHttpClient(_optionObj).
					PostAsync(SingleMessagePath, CreateSingleMessageRequest(recipient, voice, communicationContext), cancellationToken).
					ConfigureAwait(false);

				var responseContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

				if (!result.IsSuccessStatusCode)
				{
					var error = JsonConvert.DeserializeObject<RequestError>(responseContent);
					results.Add(new InfobipDispatchResult
					{
						DispatchStatus = DispatchStatus.Error,
						ResourceId = error?.serviceException?.messageId,
						Exception = new InfobipRequestErrorException(error),
					});
					Error(communicationContext, voice);
				}
				else
				{
					var success = Guard.AgainstNull(JsonConvert.DeserializeObject<SingleVoiceMessageResponse>(responseContent));
					foreach (var message in success.messages)
					{
						results.Add(new InfobipDispatchResult { ResourceId = message.messageId, DispatchStatus = ConvertStatus(message.status.groupName) });
						Dispatched(communicationContext, voice);
					}
				}
			}

			return results;
		}

		private DispatchStatus ConvertStatus(InfobipGroupName status)
		{
			return status switch
			{
				InfobipGroupName.COMPLETED or
				InfobipGroupName.PENDING or
				InfobipGroupName.IN_PROGRESS =>
					DispatchStatus.Dispatched,
				InfobipGroupName.FAILED =>
					DispatchStatus.Error,
				_ =>
					DispatchStatus.Unknown,
			};
		}

		private HttpContent CreateSingleMessageRequest(IAudienceAddress recipient, IVoice voice, IDispatchCommunicationContext context)
		{
			var language = context.CultureInfo == CultureInfo.InvariantCulture ? "en" : context.CultureInfo.TwoLetterISOLanguageName;
			var request = new SingleVoiceMessageRequest(Guard.AgainstNull(voice.Message), Guard.AgainstNullOrWhiteSpace(recipient.Value))
			{
				from = Guard.AgainstNullOrWhiteSpace(voice.From?.Value),
				language = language,
				voice = new InfobipVoiceType(voice.VoiceType)
			};

			return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
		}

		private HttpClient GetHttpClient(Configuration configuration)
		{
			if (_httpClient == null)
			{
				lock (_resourceLock)
				{
					if (_httpClient == null)
					{
						_httpClient = new HttpClient()
						{
							BaseAddress = new Uri(configuration.BasePath)
						};
						_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(configuration.ApiKeyPrefix, configuration.ApiKey);
						_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
					}
				}
			}
			return _httpClient;
		}
	}
}