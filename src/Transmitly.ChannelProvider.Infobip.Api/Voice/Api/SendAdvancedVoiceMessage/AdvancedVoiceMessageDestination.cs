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
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Voice.SendAdvancedVoiceMessage
{
	/// <summary>
	/// Message destination addresses. Destination address must be in the E.164 standard format (Example: 41793026727).
	/// </summary>
	internal sealed class AdvancedVoiceMessageDestination
	{
		public AdvancedVoiceMessageDestination(string to, string? messageId = null)
		{
			if (messageId == null)
				MessageId = Guid.NewGuid().ToString("N");
			else
				MessageId = messageId;

			To = Guard.AgainstNullOrWhiteSpace(to);

		}

		/// <summary>
		/// Message destination addresses. Destination address must be in the 
		/// E.164 standard format (Example: 41793026727).
		/// </summary>
		[JsonPropertyName("to")]
		public string To { get; }
		/// <summary>
		/// The ID that uniquely identifies the message sent.
		/// </summary>
		[JsonPropertyName("messageId")]
		public string MessageId { get; }
	}
}
