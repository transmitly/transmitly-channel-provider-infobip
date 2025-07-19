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
using System.Text.Json.Serialization;

namespace Transmitly.ChannelProvider.Infobip.Api.Voice
{
	//https://www.infobip.com/docs/api/channels/voice/voice-message/send-single-voice-tts
	sealed class SendVoiceApiSuccessResponse
	{
		/// <summary>
		/// The ID that uniquely identifies the request. 
		/// Bulk ID will be received only when you send a message to more than one destination address.
		/// </summary>
		[JsonPropertyName("bulkId")]
		public string? BulkId { get; set; }

		/// <summary>
		/// Array of sent messages, one object per every message.
		/// </summary>
		[JsonPropertyName("messages")]
		public List<SendVoiceApiResponseMessage> Messages { get; set; } = [];
	}
}