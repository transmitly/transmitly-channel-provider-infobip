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

namespace Transmitly.ChannelProvider.Infobip.Sms.SendSmsMessage
{
	/// <summary>
	/// 
	/// </summary>
	internal sealed class SendSmsMessageRequestMessageDestination(string to)
	{
		/// <summary>
		/// The ID that uniquely identifies the message sent.
		/// </summary>
		[JsonPropertyName("messageId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? MessageId { get; set; } = Guid.NewGuid().ToString("N");
		/// <summary>
		/// Message destination address. Addresses must be in international format (Example: 41793026727).
		/// </summary>
		[JsonPropertyName("to")]
		public string? To { get; set; } = Guard.AgainstNullOrWhiteSpace(to);
	}
}
