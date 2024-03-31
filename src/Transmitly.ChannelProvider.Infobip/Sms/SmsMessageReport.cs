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

using System;
using System.Text.Json.Serialization;
using Transmitly.ChannelProvider.ProviderResponse;
namespace Transmitly.ChannelProvider.Infobip.Sms
{
	sealed class SmsMessageReport : ChannelProviderReport
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

		[JsonPropertyName("bulkId")]
		public string? BulkId { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(BulkId)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(BulkId), value); }

		[JsonPropertyName("messageId")]
		public string? MessageId { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(MessageId)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(MessageId), value); }

		[JsonPropertyName("to")]
		public string? To { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(To)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(To), value); }

		[JsonPropertyName("from")]
		public string? From { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(From)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(From), value); }

		[JsonPropertyName("sentAt")]
		public DateTimeOffset? SentAt { get => ExtendedProperties.GetValue<DateTimeOffset?>(Constant.SmsPropertiesKey, nameof(SentAt)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(SentAt), value); }

		[JsonPropertyName("doneAt")]
		public DateTimeOffset? DoneAt { get => ExtendedProperties.GetValue<DateTimeOffset?>(Constant.SmsPropertiesKey, nameof(DoneAt)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(DoneAt), value); }

		[JsonPropertyName("smsCount")]
		public int? SmsCount { get => ExtendedProperties.GetValue<int?>(Constant.SmsPropertiesKey, nameof(SmsCount)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(SmsCount), value); }

		[JsonPropertyName("mccMnc")]
		public string? MccMnc { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(MccMnc)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(MccMnc), value); }

		[JsonPropertyName("callbackData")]
		public string? CallbackData { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(CallbackData)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(CallbackData), value); }

		[JsonPropertyName("price")]
		public SmsPrice? Price { get => ExtendedProperties.GetValue<SmsPrice?>(Constant.SmsPropertiesKey, nameof(Price)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(Price), value); }

		[JsonPropertyName("status")]
		public CallbackStatus? StatusDetail { get => ExtendedProperties.GetValue<CallbackStatus?>(Constant.SmsPropertiesKey, nameof(StatusDetail)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(StatusDetail), value); }

		[JsonPropertyName("error")]
		public ErrorStatus? Error { get => ExtendedProperties.GetValue<ErrorStatus?>(Constant.SmsPropertiesKey, nameof(Error)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(Error), value); }

		[JsonPropertyName("entityId")]
		public string? EntityId { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(EntityId)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(EntityId), value); }

		[JsonPropertyName("applicationId")]
		public string? ApplicationId { get => ExtendedProperties.GetValue<string?>(Constant.SmsPropertiesKey, nameof(ApplicationId)); set => ExtendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(ApplicationId), value); }
	}
}