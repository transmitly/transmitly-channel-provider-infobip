﻿// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
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

namespace Transmitly.ChannelProvider.Infobip.Email
{
	sealed class EmailStatusReport
	{
		[JsonPropertyName("bulkId")]
		public string? BulkId { get; set; }

		[JsonPropertyName("messageId")]
		public string? MessageId { get; set; }

		[JsonPropertyName("to")]
		public string? To { get; set; }

		[JsonPropertyName("sentAt")]
		[JsonConverter(typeof(InfobipDateTimeOffsetConverter))]
		public DateTimeOffset? SentAt { get; set; }

		[JsonPropertyName("doneAt")]
		[JsonConverter(typeof(InfobipDateTimeOffsetConverter))]
		public DateTimeOffset? DoneAt { get; set; }

		[JsonPropertyName("smsCount")]
		public int? SmsCount { get; set; }

		[JsonPropertyName("callbackData")]
		public string? CallbackData { get; set; }

		[JsonPropertyName("price")]
		public EmailPrice? Price { get; set; }

		[JsonPropertyName("status")]
		public CallbackStatus? Status { get; set; }

		[JsonPropertyName("error")]
		public ErrorStatus? Error { get; set; }

		[JsonPropertyName("browserLink")]
		public string? BrowserLink { get; set; }
	}
}