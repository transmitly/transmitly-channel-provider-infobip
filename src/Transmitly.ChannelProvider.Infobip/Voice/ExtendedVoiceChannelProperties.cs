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
	public sealed class ExtendedVoiceChannelProperties
	{
		private readonly IExtendedProperties _extendedProperties;
		private const string ProviderKey = Constant.VoicePropertiesKey;
		internal ExtendedVoiceChannelProperties(IVoiceChannel voiceChannel)
		{
			Guard.AgainstNull(voiceChannel);
			_extendedProperties = Guard.AgainstNull(voiceChannel.ExtendedProperties);
		}
		internal ExtendedVoiceChannelProperties(IExtendedProperties properties)
		{
			_extendedProperties = Guard.AgainstNull(properties);
		}

		/// <summary>
		/// Maximum possible duration of the call to be set, shown in seconds.
		/// </summary>
		public int? CallTimeout
		{
			get => _extendedProperties.GetValue<int?>(ProviderKey, nameof(CallTimeout));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(CallTimeout), value);
		}

		/// <summary>
		/// Defines the maximum number of DTMF codes entered by end user that would be collected.
		/// </summary>
		public int? MaxDtmf
		{
			get => _extendedProperties.GetValue<int?>(ProviderKey, nameof(MaxDtmf));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(MaxDtmf), value);
		}
		/// <summary>
		/// Period of time in seconds between end user answering the call and message 
		/// starting to be played. Minimal value is 0 and maximum value is 10 seconds.
		/// Default value is 0.
		/// </summary>
		public int? Pause
		{
			get => _extendedProperties.GetValue<int?>(ProviderKey, nameof(Pause));
			set
			{
				if (value < 0 || value > 10)
					throw new InfobipException($"{nameof(Pause)} value must be > 0 and < 10 seconds.");
				_extendedProperties.AddOrUpdate(ProviderKey, nameof(Pause), value);
			}
		}

		/// <summary>
		/// Record the call and expose it to client as URL inside the delivery report. Can be true or false.
		/// <seealso cref="https://www.infobip.com/docs/voice-and-video/add-ons#recording"/>
		/// </summary>
		public bool? Record
		{
			get => _extendedProperties.GetValue<bool?>(ProviderKey, nameof(Record));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Record), value);
		}
		/// <summary>
		/// Ringing duration, unless there are no operator limitations. Default value is 45. 
		/// Note: There are no limitations on the Voice platform regarding this value, however, 
		/// most of the operators have their own ring timeout limitations and it is advisable 
		/// to keep the ringTimeout value up to 45 seconds
		/// </summary>
		public int? RingTimeout
		{
			get => _extendedProperties.GetValue<int?>(ProviderKey, nameof(RingTimeout));
			set
			{
				if (value < 0 || value > 45)
					throw new InfobipException($"{nameof(RingTimeout)} value must be > 0 and < 45 seconds.");
				_extendedProperties.AddOrUpdate(ProviderKey, nameof(RingTimeout), value);
			}
		}

		/// <summary>
		/// Name of the voice. Example: Joanna
		/// </summary>
		public string? VoiceName
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(VoiceName));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(VoiceName), value);
		}
		/// <summary>
		/// Gender of the voice. Can be male or female.
		/// </summary>
		public VoiceGender VoiceGender
		{
			get => _extendedProperties.GetValue(ProviderKey, nameof(VoiceGender), VoiceGender.Unspecified);
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(VoiceGender), value);
		}
		/// <summary>
		/// Used for enabling detection of answering machine after the call has been answered.
		/// 
		/// When set to hangup, if a machine is detected call will hang up. When set to continue, 
		/// if a machine is detected, then voice message starts playing into voice mail after the 
		/// answering message is finished with its greeting. If machineDetection is used, there is a minimum of 4 
		/// seconds detection time, which can result in delay of playing the message. Answering machine detection 
		/// is additionally charged. For more information please contact your account manager and check 
		/// documentation on Answering Machine Detection.
		/// <see href="https://www.infobip.com/docs/voice-and-video/add-ons#answering-machine-detection">Answering Machine Detection</see>
		/// </summary>
		public MachineDetection MachineDetection
		{
			get => _extendedProperties.GetValue(ProviderKey, nameof(MachineDetection), MachineDetection.HangUp);
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(MachineDetection), value);
		}

		/// <summary>
		/// The URL on your callback server on which the Delivery report will be sent.
		/// </summary>
		public string? NotifyUrl
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(NotifyUrl));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(NotifyUrl), value);
		}
	}
}