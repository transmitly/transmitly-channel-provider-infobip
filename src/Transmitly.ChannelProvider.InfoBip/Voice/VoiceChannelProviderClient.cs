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
using System.Text;
using Transmitly.Infobip;
using System.Text.Json;

namespace Transmitly.ChannelProvider.Infobip.Voice
{
	internal sealed class VoiceChannelProviderClient(InfobipChannelProviderConfiguration configuration) : ChannelProviderRestClient<IVoice>(null)
	{
		private const string SendSingleVoiceTts = "tts/3/single";
		private readonly InfobipChannelProviderConfiguration _configuration = configuration;

		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

		protected override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, IVoice communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(communication);
			Guard.AgainstNull(communicationContext);

			var recipients = communication.To ?? [];

			var results = new List<IDispatchResult>(recipients.Length);

			foreach (var recipient in recipients)
			{
				var result = await restClient
					.PostAsync(
						SendSingleVoiceTts,
						CreateSingleMessageRequest(recipient, communication, communicationContext),
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
					var success = Guard.AgainstNull(JsonSerializer.Deserialize<SendSingleVoiceTtsResponse>(responseContent));
					foreach (var message in success.Messages)
					{
						results.Add(new InfobipDispatchResult { ResourceId = message.MessageId, DispatchStatus = ConvertStatus(message.Status.GroupName) });
						Dispatched(communicationContext, communication);
					}
				}
			}

			return results;
		}

		private HttpContent CreateSingleMessageRequest(IAudienceAddress recipient, IVoice voice, IDispatchCommunicationContext context)
		{
			var voiceProperties = new ExtendedVoiceChannelProperties(voice.ExtendedProperties);

			Guard.AgainstNull(voice.From);

			var request = new SendSingleVoiceTtsRequest(voice.Message, voice.From.Value, recipient.Value)
			{
				Language = context.CultureInfo.TwoLetterISOLanguageNameDefault(),
				Voice = new InfobipVoiceType(voice.VoiceType, voiceProperties.VoiceGender, voiceProperties.VoiceName),
			};

			return new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
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

		protected override void ConfigureHttpClient(HttpClient client)
		{
			RestClientConfiguration.Configure(client, _configuration);
			base.ConfigureHttpClient(client);
		}
	}
}