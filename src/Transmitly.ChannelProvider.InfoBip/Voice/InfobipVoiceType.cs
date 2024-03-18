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

namespace Transmitly.ChannelProvider.Infobip.Voice
{
	/// <summary>
	/// <see href="https://www.infobip.com/docs/voice-and-video/getting-started#text-to-speech"/>
	/// </summary>
	internal sealed class InfobipVoiceType
	{
		public InfobipVoiceType(IVoiceType? voiceType) : this(voiceType, VoiceGender.Unspecified, null)
		{
			if (voiceType == null)
				return;
			Gender = voiceType.Gender;
			Name = voiceType.Name;
		}

		public InfobipVoiceType(IVoiceType? voiceType, VoiceGender overrideGender, string? overrideName)
		{
			if (overrideGender != VoiceGender.Unspecified)
				Gender = Enum.GetName(typeof(VoiceGender), overrideGender);
			else
				Gender = voiceType?.Gender;

			if (!string.IsNullOrEmpty(overrideName))
				Name = overrideName;
			else
				Name = voiceType?.Name;
		}

		public string? Gender { get; set; }
		public string? Name { get; set; }
	}
}