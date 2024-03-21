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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Transmitly.ChannelProvider.Infobip.Voice.SendAdvancedVoiceMessage
{
	internal sealed class SendAdvancedVoiceMessageRequest(List<AdvancedVoiceMessage> messages, string bulkId)
	{
		/// <summary>
		/// The ID which uniquely identifies the request.
		/// </summary>
		[JsonPropertyName("bulkId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string BulkId { get; } = bulkId;
		/// <summary>
		/// Array of messages to be sent, one object per every message.
		/// </summary>
		[JsonPropertyName("messages")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IReadOnlyCollection<AdvancedVoiceMessage> Messages { get; } = Guard.AgainstNullOrEmpty(messages);
	}
}
