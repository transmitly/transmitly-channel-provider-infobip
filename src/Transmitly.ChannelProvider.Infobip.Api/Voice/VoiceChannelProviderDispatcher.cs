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
using Transmitly.ChannelProvider.Infobip.Voice.SendAdvancedVoiceMessage;
using System;
using Transmitly.Delivery;
using Transmitly.ChannelProvider.Infobip.Configuration;
namespace Transmitly.ChannelProvider.Infobip.Voice
{
    public sealed class VoiceChannelProviderDispatcher(InfobipChannelProviderConfiguration configuration) : ChannelProviderRestDispatcher<IVoice>(null)
    {
        private const string AdvancedCallEndpoint = "tts/3/advanced";
        private readonly InfobipChannelProviderConfiguration _configuration = configuration;

        protected override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, IVoice communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
        {
            Guard.AgainstNull(communication);
            Guard.AgainstNull(communicationContext);
            Guard.AgainstNullOrWhiteSpace(communication.From?.Value);

            var recipients = communication.To ?? [];

            var results = new List<IDispatchResult>(recipients.Length);

            foreach (var recipient in recipients)
            {
                Dispatch(communicationContext, communication);

                var result = await restClient
                    .PostAsync(
                        AdvancedCallEndpoint,
                        await CreateAdvancedMessagePayloadAsync(recipient, communication, communicationContext).ConfigureAwait(false),
                        cancellationToken
                    )
                    .ConfigureAwait(false);

                var responseContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!result.IsSuccessStatusCode)
                {
                    var error = JsonSerializer.Deserialize<ApiRequestErrorResult>(responseContent);
                    results.Add(new InfobipDispatchResult
                    {
                        DispatchStatus = DispatchStatus.Exception,
                        ResourceId = error?.ServiceException?.MessageId,
                        Exception = new ApiResultException(error),
                    });
                    Error(communicationContext, communication, results);
                }
                else
                {
                    var success = Guard.AgainstNull(JsonSerializer.Deserialize<SendVoiceApiSuccessResponse>(responseContent));
                    foreach (var message in success.Messages)
                    {
                        results.Add(new InfobipDispatchResult
                        {
                            ResourceId = message.MessageId,
                            DispatchStatus = ConvertStatus(message.Status.GroupName),
                            BulkId = success.BulkId
                        });

                        Dispatched(communicationContext, communication, results);
                    }
                }
            }

            return results;
        }

        private async Task<HttpContent> CreateAdvancedMessagePayloadAsync(IIdentityAddress recipient, IVoice voice, IDispatchCommunicationContext context)
        {
            var voiceProperties = new VoiceExtendedChannelProperties(voice.ExtendedProperties);
            var messageId = Guid.NewGuid().ToString("N");
            var bulkId = Guid.NewGuid().ToString("N");

            var request = new AdvancedVoiceMessage(recipient.Value, messageId)
            {
                Text = voice.Message,
                From = voice.From?.Value,
                MachineDetection = voice.MachineDetection,//ConvertMachineDetection(voice.MachineDetection, voiceProperties.MachineDetection),
                NotifyUrl = await GetNotifyUrl(messageId, voiceProperties, voice, context).ConfigureAwait(false),
                CallTimeout = voiceProperties.CallTimeout,
                Language = context.CultureInfo.TwoLetterISOLanguageNameDefault(),
                VoiceType = new InfobipVoiceType(voice.VoiceType, voiceProperties.VoiceGender, voiceProperties.VoiceName).ToObject(),

            };
            var message = new SendAdvancedVoiceMessageRequest([request], bulkId);
            return new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
        }

        //private MachineDetection? ConvertMachineDetection(Transmitly.MachineDetection tlyValue, MachineDetection? overrideValue)
        //{
        //	if (overrideValue.HasValue)
        //		return overrideValue;

        //	return tlyValue switch
        //	{
        //		Transmitly.MachineDetection.Disabled =>
        //			(MachineDetection?)MachineDetection.HangUp,
        //		Transmitly.MachineDetection.Enabled or Transmitly.MachineDetection.MessageEnd =>
        //			(MachineDetection?)MachineDetection.Continue,
        //		_ => null,
        //	};
        //}

        private static async Task<string?> GetNotifyUrl(string messageId, VoiceExtendedChannelProperties smsProperties, IVoice voice, IDispatchCommunicationContext context)
        {
            string? url;
            var urlResolver = smsProperties.NotifyUrlResolver ?? voice.DeliveryReportCallbackUrlResolver;
            if (urlResolver != null)
                url = await urlResolver(context).ConfigureAwait(false);
            else
            {
                url = smsProperties.NotifyUrl ?? voice.DeliveryReportCallbackUrl;
                if (string.IsNullOrWhiteSpace(url))
                    return null;
            }
            return new Uri(url).AddPipelineContext(messageId, context.PipelineName, context.ChannelId, context.ChannelProviderId).ToString();
        }

        private static DispatchStatus ConvertStatus(InfobipGroupName status)
        {
            return status switch
            {
                InfobipGroupName.COMPLETED or
                InfobipGroupName.PENDING or
                InfobipGroupName.IN_PROGRESS =>
                    DispatchStatus.Dispatched,
                InfobipGroupName.FAILED =>
                    DispatchStatus.Exception,
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