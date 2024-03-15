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

namespace Transmitly.ChannelProvider.Infobip.Voice
{
	sealed class SingleVoiceMessageRequest
	{
		public SingleVoiceMessageRequest(string text, string to)
		{
			this.text = text;
			this.to = to;
		}
		public string text { get; }
		public string language { get; set; } = "en";
		public string? from { get; set; }
		public string to { get; set; }
		public InfobipVoiceType? voice { get; set; }
	}
}