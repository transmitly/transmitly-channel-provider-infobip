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
using Transmitly.ChannelProvider.Infobip.Configuration.Voice;

namespace Transmitly.ChannelProvider.Infobip.Api.Voice
{
	/// <summary>
	/// Fields representing details specific for voice messages.
	/// </summary>
	sealed class VoiceCallDetail : IVoiceCallDetail
	{
		/// <summary>
		/// Name of the Infobip Voice service or feature.
		/// </summary>
		[JsonPropertyName("feature")]
		public string? Feature { get; set; }
		/// <summary>
		/// Date and time when the voice message was established and started ringing. 
		/// Has the following format: yyyy-MM-dd'T'HH:mm:ss.SSSZ.
		/// </summary>
		[JsonPropertyName("startTime")]
		[JsonConverter(typeof(InfobipDateTimeOffsetConverter))]
		public DateTimeOffset? StartTime { get; set; }
		/// <summary>
		/// Date and time when the voice message was answered. 
		/// Has the following format: yyyy-MM-dd'T'HH:mm:ss.SSSZ.
		/// </summary>
		[JsonPropertyName("answerTime")]
		[JsonConverter(typeof(InfobipDateTimeOffsetConverter))]
		public DateTimeOffset? AnswerTime { get; set; }
		/// <summary>
		/// Date and time when the voice message was ended. 
		/// Has the following format: yyyy-MM-dd'T'HH:mm:ss.SSSZ.
		/// </summary>
		[JsonPropertyName("endTime")]
		[JsonConverter(typeof(InfobipDateTimeOffsetConverter))]
		public DateTimeOffset? EndTime { get; set; }
		/// <summary>
		/// Duration of the voice message, in seconds.
		/// </summary>
		[JsonPropertyName("duration")]
		public int? Duration { get; set; }
		/// <summary>
		/// Charged duration of the voice message, in seconds.
		/// </summary>
		[JsonPropertyName("chargeDuration")]
		public int? ChargedDuration { get; set; }
		/// <summary>
		/// Duration of the voice message audio file, in seconds.
		/// </summary>
		[JsonPropertyName("fileDuration")]
		public double? FileDuration { get; set; }
		/// <summary>
		/// DTMF code entered by user. Can be empty string, if user did not press anything, 
		/// or null in case of IVR if user did not participate in Collect action
		/// </summary>
		[JsonPropertyName("dtmfCodes")]
		public string? DtmfCodes { get; set; }
		/// <summary>
		/// Fields representing details about IVR, if outbound IVR is being used. 
		/// If the call was not IVR, the field will be null.
		/// </summary>
		[JsonPropertyName("ivr")]
		public object? Ivr { get; set; }
	}
}