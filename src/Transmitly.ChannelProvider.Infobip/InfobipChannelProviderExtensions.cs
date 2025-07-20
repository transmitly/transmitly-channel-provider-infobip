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
using Transmitly.ChannelProvider.Infobip.Api.Email;
using Transmitly.ChannelProvider.Infobip.Api.Sms;
using Transmitly.ChannelProvider.Infobip.Api.Voice;
using Transmitly.ChannelProvider.Infobip.Configuration;

namespace Transmitly
{
	public static class InfobipChannelProviderExtensions
	{
		/// <summary>
		/// Adds channel provider support for Infobip.
		/// </summary>
		/// <param name="communicationsClientBuilder">Communications builder.</param>
		/// <param name="options">Infobip channel provider options and settings.</param>
		/// <param name="providerId">Optional channel provider Id.</param>
		/// <returns></returns>
		public static CommunicationsClientBuilder AddInfobipSupport(this CommunicationsClientBuilder communicationsClientBuilder, Action<InfobipChannelProviderConfiguration> options, string? providerId = null)
		{
			var optionObj = new InfobipChannelProviderConfiguration();
			options(optionObj);

			communicationsClientBuilder.ChannelProvider.Build(Id.ChannelProvider.Infobip(providerId), optionObj)
				.AddDispatcher<SmsChannelProviderDispatcher, ISms>(Id.Channel.Sms())
				.AddDispatcher<EmailChannelProviderDispatcher, IEmail>(Id.Channel.Email())
				.AddDispatcher<VoiceChannelProviderDispatcher, IVoice>(Id.Channel.Voice())
				.AddDeliveryReportRequestAdaptor<SmsDeliveryStatusReportAdaptor>()
				.AddDeliveryReportRequestAdaptor<VoiceDeliveryStatusReportAdaptor>()
				.AddDeliveryReportRequestAdaptor<EmailDeliveryStatusReportAdaptor>()
				.AddSmsExtendedPropertiesAdaptor<SmsExtendedChannelProperties>()
				.AddVoiceExtendedPropertiesAdaptor<VoiceExtendedChannelProperties>()
				.AddEmailExtendedPropertiesAdaptor<EmailExtendedChannelProperties>()
				.Register();

			return communicationsClientBuilder;
		}
	}
}