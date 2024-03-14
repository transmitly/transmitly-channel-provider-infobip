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
using System.Linq;

namespace Transmitly.Infobip
{
	sealed class SingleVoiceMessageRequest
	{
		public SingleVoiceMessageRequest(string text)
		{
			this.text = text;
		}
		public string text { get; set; }
		public string language { get; set; } = "en";
		public string from { get; set; }
		public string to { get; set; }
	}

	sealed class SingleVoiceMessageResponse
	{
		public string bulkId { get; set; }
		public List<string> Messages { get; set; } = [];
	}
	sealed class VoiceMessageStatus
	{
		public int groupId { get; set; }
		public string groupName { get; set; }
		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
	}

	sealed class SingleVoiceMessage
	{
		public string to { get; set; }
		public string messageId { get; set; }
		public VoiceMessageStatus status { get; set; } = new VoiceMessageStatus();
	}

	internal sealed class InfobipVoiceChannelProviderClient : ChannelProviderClient<IVoice>
	{
		private const string SingleMessagePath = "tts/v3/single";
		private static HttpClient? _httpClient;
		private static readonly object _resourceLock = new();
		private readonly Configuration _optionObj;

		public InfobipVoiceChannelProviderClient(IB.Configuration optionObj)
		{
			_optionObj = Guard.AgainstNull(optionObj);
		}

		private static HttpClient GetHttpClient(Configuration configuration)
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
						_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
						_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
					}
				}
			}
			return _httpClient;
		}

		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

		public override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(IVoice voice, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(voice);
			Guard.AgainstNull(communicationContext);
			var messageId = Guid.NewGuid().ToString("N");
			Guard.AgainstNull(voice.From);
			Guard.AgainstNull(voice.To);

			var result = await GetHttpClient(_optionObj).PostAsync(SingleMessagePath, new StringContent(JsonConvert.SerializeObject(new SingleVoiceMessageRequest("")
			{
				to = voice.To.First().Value,
				from = voice.From.Value
			})));
			return [new DispatchResult(DispatchStatus.Dispatched, messageId)];
		}
	}
}