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
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Voice
{
	/// <summary>
	/// Send a single voice message to one or more destination addresses. 
	/// <see href="https://www.infobip.com/docs/api/channels/voice/voice-message/send-single-voice-tts">Send single voice message Documentation</see>
	/// </summary>
	/// <param name="text">Message to be converted to speech and played to subscribers.</param>
	/// <param name="from">Numeric sender ID in E.164 standard format.</param>
	/// <param name="to">Phone number of the recipient.</param>
	sealed class SendSingleVoiceTtsRequest(string text, string from, string to)
	{
		/// <summary>
		/// Message to be converted to speech and played to subscribers. Message text can be up to 1400 characters long and cannot contain only punctuation. 
		/// SSML (Speech Synthesis Markup Language) is supported and can be used to fully customize pronunciation of the provided text.
		/// </summary>
		[JsonPropertyName("text")]
		public string Text { get; } = Guard.AgainstNullOrWhiteSpace(text).Length > 1400 ? throw new InfobipException("Voice message text cannot be greater than 1400 characters") : text;

		/// <summary>
		/// If the message is in text format, the language in which the message is written must be defined for correct pronunciation. 
		/// More about Text-to-speech functionality and supported TTS languages can be found here. If not set, default language is English [en]. 
		/// If voice is not set, then default voice for that specific language is used. 
		/// In the case of English language, the voice is [Joanna].
		/// <seealso href="https://www.infobip.com/docs/voice-and-video/getting-started#text-to-speech">Voice Text-To-Speech Languages</seealso>
		/// </summary>
		[JsonPropertyName("language")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Language { get; set; }

		/// <summary>
		/// Numeric sender ID in E.164 standard format (Example: 41793026727). 
		/// This is caller ID that will be presented to the end user where applicable.
		/// </summary>
		[JsonPropertyName("from")]
		public string? From { get; } = Guard.AgainstNullOrWhiteSpace(from);

		/// <summary>
		/// Phone number of the recipient. 
		/// Phone number must be written in E.164 standard format (Example: 41793026727).
		/// </summary>
		[JsonPropertyName("to")]
		public string To { get; } = Guard.AgainstNullOrWhiteSpace(to);

		/// <summary>
		/// Used to define voice in which text would be synthesized. It has two parameters: name and gender. 
		/// When only name is provided, then that exact voice with that name will be used to synthesize text. 
		/// If only gender is provided, then text is synthesized with first voice in given gender. 
		/// If voice is not set, then default voice is used
		/// </summary>
		[JsonPropertyName("voice")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InfobipVoiceType? Voice { get; set; }

		/// <summary>
		/// An audio file can be delivered as a voice message to the recipients. 
		/// An audio file must be uploaded online, so that the existing URL can be available for file download. 
		/// Size of the audio file must be below 4 MB. Supported formats of the provided file are mp3 and wav. 
		/// Our platform needs to have permission to make GET and HEAD HTTP requests on the provided URL. 
		/// Standard http ports (like 80, 8080, etc.) are advised.
		/// </summary>
		[JsonPropertyName("audioFileUrl")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? AudioFileUrl { get; set; }
	}
}