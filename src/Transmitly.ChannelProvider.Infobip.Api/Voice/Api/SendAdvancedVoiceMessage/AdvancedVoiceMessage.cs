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
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Voice.SendAdvancedVoiceMessage
{
	internal sealed class AdvancedVoiceMessage
	{
		private string? _callbackData;
		private int? _ringTimeout;
		private string? _text;

		public AdvancedVoiceMessage(string to, string? messageId = null) : this([new AdvancedVoiceMessageDestination(to, messageId)])
		{

		}

		public AdvancedVoiceMessage(List<AdvancedVoiceMessageDestination> destinations)
		{
			Guard.AgainstNullOrEmpty(destinations);
			if (destinations.Count > 20000)
				throw new InfobipException($"{nameof(destinations)} addresses exceeds maximum of 20000 records");

			Destinations = destinations;

		}

		/// <summary>
		/// An audio file can be delivered as a voice message to the recipients. An audio file must 
		/// be uploaded online, so that the existing URL can be available for file download. 
		/// Size of the audio file must be below 4 MB. Supported formats of the provided file are 
		/// aac, aiff, m4a, mp2, mp3, mp4 (audio only), ogg, wav and wma. Our platform needs to have 
		/// permission to make GET and HEAD HTTP requests on the provided URL. 
		/// Standard http ports (like 80, 8080, etc.) are advised
		/// </summary>
		[JsonPropertyName("audioFileUrl")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? AudioFileUrl { get; set; }

		/// <summary>
		/// Maximum possible duration of the call to be set, shown in seconds.
		/// </summary>
		[JsonPropertyName("callTimeout")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? CallTimeout { get; set; }

		/// <summary>
		/// Maximum possible duration of the call to be set, shown in seconds.
		/// </summary>
		/// <exception cref="InfobipException">When data length is greater than 200 characters.</exception>
		[JsonPropertyName("callbackData")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? CallbackData
		{
			get => _callbackData;
			set
			{
				if (!string.IsNullOrWhiteSpace(value) && value.Length > 200)
					throw new InfobipException($"{nameof(CallbackData)} data length cannot be greater than 200.");

				_callbackData = value;
			}
		}
		/// <summary>
		/// Message destination addresses. Destination address must be in the E.164 standard format (Example: 41793026727). 
		/// Maximum number of destination addresses is 20k
		/// </summary>
		[JsonPropertyName("destinations")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IReadOnlyCollection<AdvancedVoiceMessageDestination> Destinations { get; }

		/// <summary>
		/// The waiting period for end user to enter DTMF digits. Default value is 10 seconds.
		/// </summary>
		[JsonPropertyName("dtmfTimeout")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? DtmfTimeout { get; set; }

		/// <summary>
		/// Numeric sender ID in E.164 standard format (Example: 41793026727). 
		/// This is caller ID that will be presented to the end user where applicable
		/// </summary>
		[JsonPropertyName("from")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? From { get; set; }

		/// <summary>
		/// If the message is in text format, the language in which the message is written must be defined 
		/// for correct pronunciation. More about Text-to-speech functionality and supported TTS languages 
		/// can be found <see href="https://www.infobip.com/docs/voice-and-video/outbound-calls#text-to-speech-voice-over-broadcast">here</see>. 
		/// If not set, default language is <em>English [en]</em>. If voice is not set, then default voice for that specific language is used. 
		/// In the case of English language, the voice is <em>[Joanna]</em>.
		/// <see href="https://www.infobip.com/docs/voice-and-video/getting-started#text-to-speech">Voice Text-To-Speech Languages</see>
		/// </summary>
		[JsonPropertyName("language")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Language { get; set; }

		/// <summary>
		/// Used for enabling detection of answering machine after the call has been answered. 
		/// It can be set to <em>hangup</em> or <em>continue</em>. When set to <em>hangup</em>, if a machine is detected call will hang up. 
		/// When set to <em>continue</em>, if a machine is detected, then voice message starts playing into voice mail after 
		/// the answering message is finished with its greeting. If machineDetection is used, there is a minimum of 4 seconds 
		/// detection time, which can result in delay of playing the message. Answering machine detection is additionally charged. 
		/// For more information please contact your account manager and check documentation on Answering Machine Detection.
		/// <see href="https://www.infobip.com/docs/voice-and-video/add-ons#answering-machine-detection">Answering Machine Detection</see>
		/// </summary>
		[JsonPropertyName("machineDetection")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public MachineDetection? MachineDetection { get; set; }

		/// <summary>
		/// Preferred Delivery report content type. Can be <em>application/json</em> or <em>application/xml</em>. 
		/// <see href="https://www.infobip.com/docs/api#channels/voice/get-voice-delivery-reports">Read more</see>
		/// </summary>
		[JsonPropertyName("notifyContentVersion")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? NotifyContentVersion { get; set; }

		/// <summary>
		/// The URL on your callback server on which the Delivery report will be sent.
		/// </summary>
		[JsonPropertyName("notifyUrl")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? NotifyUrl { get; set; }

		/// <summary>
		/// Ringing duration, unless there are no operator limitations. Default value is 45. Note: 
		/// There are no limitations on the Voice platform regarding this value, however, most of the 
		/// operators have their own ring timeout limitations and it is advisable to keep the 
		/// ringTimeout value up to 45 seconds.
		/// </summary>
		[JsonPropertyName("ringTimeout")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? RingTimeout
		{
			get => _ringTimeout;
			set
			{
				if (value.HasValue && value > 45)
					throw new InfobipException($"{nameof(RingTimeout)} cannot exceed 45 seconds");
				_ringTimeout = value;
			}
		}

		/// <summary>
		/// Text of the message that will be sent. Message text can be up to 1400 characters long and cannot contain only punctuation. 
		/// Adding pauses between the words and extending the duration of the voice message is possible by using the comma character “,”. 
		/// For example, if you want to have a 3 second pause after each word, then the text parameter should look like this 
		/// <em>“one,,,,,,two,,,,,,three,,,,,,”</em>. 
		/// Each comma creates a pause of 0.5 seconds. SSML (Speech Synthesis Markup Language) is supported and can be used 
		/// to fully customize pronunciation of the provided text.
		/// <see href="https://www.infobip.com/docs/voice-and-video/getting-started">SSML support</see>
		/// </summary>
		[JsonPropertyName("text")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? Text
		{
			get => _text;
			set
			{
				if (!string.IsNullOrWhiteSpace(value) && value!.Length > 1400)
					throw new InfobipException($"{nameof(Text)} cannot exceed 1400 characters.");
				_text = value;
			}
		}
		/// <summary>
		/// The message validity period shown in minutes. When the period expires, it will not be allowed for the message to be sent. 
		/// A validity period longer than 48h is not supported (in this case, it will be automatically set to 48h).
		/// </summary>
		[JsonPropertyName("validityPeriod")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? ValidityPeriod { get; set; }

		/// <summary>
		/// Used to define voice in which text would be synthesized. It has two parameters: name and gender. When only name is provided, 
		/// then that exact voice with that name will be used to synthesize text. If only gender is provided, then text is synthesized with 
		/// first voice in given gender. If voice is not set, then default voice is used.
		/// </summary>
		[JsonPropertyName("voiceType")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public InfobipVoiceType? VoiceType { get; set; }
	}
}
