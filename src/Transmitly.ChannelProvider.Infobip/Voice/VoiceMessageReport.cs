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

using System.Text.Json.Serialization;
using Transmitly.ChannelProvider.ProviderResponse;
namespace Transmitly.ChannelProvider.Infobip.Voice
{
    sealed class VoiceMessageReport : ChannelProviderReport
    {
        public override string? Id => MessageId;

        public override DispatchStatus Status
        {
            get
            {
                return (StatusDetail?.GroupId) switch
                {
                    //PENDING
                    1 => DispatchStatus.Dispatched,
                    //UNDELIVERABLE
                    2 or 4 or 5 => DispatchStatus.Undeliverable,
                    //DELIVERED
                    3 => DispatchStatus.Delivered,
                    _ => DispatchStatus.Unknown,
                };
            }
        }

        [JsonPropertyName("messageId")]
        public string? MessageId { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(MessageId)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(MessageId), value); }

        [JsonPropertyName("feature")]
        public string? Feature { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(Feature)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(Feature), value); }

        [JsonPropertyName("status")]
        public CallbackStatus? StatusDetail { get => ExtendedProperties.GetValue<CallbackStatus?>(Constant.SmsPropertiesKey, nameof(StatusDetail)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(StatusDetail), value); }

        [JsonPropertyName("error")]
        public ErrorStatus? Error { get => ExtendedProperties.GetValue<ErrorStatus?>(Constant.SmsPropertiesKey, nameof(Error)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(Error), value); }
    }
}