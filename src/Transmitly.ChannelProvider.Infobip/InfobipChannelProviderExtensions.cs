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
using Transmitly.Delivery;
using Transmitly.ChannelProvider.Infobip.Email;
using Transmitly.ChannelProvider.Infobip.Sms;
using Transmitly.ChannelProvider.Infobip.Voice;

namespace Transmitly
{
	public static class InfobipChannelProviderExtensions
	{
		/// <summary>
		/// Gets the channel provider id for Infobip.
		/// </summary>
		/// <param name="channelProviders">Channel providers object.</param>
		/// <param name="providerId">Optional channel provider Id.</param>
		/// <returns>Infobip channel provider id.</returns>
		public static string Infobip(this ChannelProviders channelProviders, string? providerId = null)
		{
			Guard.AgainstNull(channelProviders);
			return channelProviders.GetId(Constant.Id, providerId);
		}

		/// <summary>
		/// Infobip specific settings for email channels.
		/// </summary>
		/// <param name="sms">Email Channel.</param>
		/// <returns>Infobip email properties.</returns>
		public static ExtendedEmailChannelProperties Infobip(this IEmailChannel email)
		{
			return new ExtendedEmailChannelProperties(email);
		}

		/// <summary>
		/// Infobip specific settings for Sms channels.
		/// </summary>
		/// <param name="sms">Sms Channel.</param>
		/// <returns>Infobip Sms properties.</returns>
		public static ExtendedSmsChannelProperties Infobip(this ISmsChannel sms)
		{
			return new ExtendedSmsChannelProperties(sms);
		}

		/// <summary>
		/// Infobip specific settings for voice channels.
		/// </summary>
		/// <param name="sms">Voice Channel.</param>
		/// <returns>Infobip voice properties.</returns>
		public static ExtendedVoiceChannelProperties Infobip(this IVoiceChannel email)
		{
			return new ExtendedVoiceChannelProperties(email);
		}

		/// <summary>
		/// Infobip specific settings for sms delivery reports.
		/// </summary>
		/// <param name="deliveryReport">Delivery Report.</param>
		/// <returns>Infobip SMS delivery report properties.</returns>
		public static DeliveryReportExtendedProperties Infobip(this DeliveryReport deliveryReport)
		{
			return new DeliveryReportExtendedProperties(deliveryReport);
		}

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

			communicationsClientBuilder.AddChannelProvider<SmsChannelProviderClient, ISms>(Id.ChannelProvider.Infobip(providerId), optionObj, Id.Channel.Sms());
			communicationsClientBuilder.AddChannelProvider<EmailChannelProviderClient, IEmail>(Id.ChannelProvider.Infobip(providerId), optionObj, Id.Channel.Email());
			communicationsClientBuilder.AddChannelProvider<VoiceChannelProviderClient, IVoice>(Id.ChannelProvider.Infobip(providerId), optionObj, Id.Channel.Voice());
			communicationsClientBuilder.ChannelProvider.AddDeliveryReportRequestAdaptor<SmsDeliveryStatusReportAdaptor>();
			communicationsClientBuilder.ChannelProvider.AddDeliveryReportRequestAdaptor<VoiceDeliveryStatusReportAdaptor>();
			communicationsClientBuilder.ChannelProvider.AddDeliveryReportRequestAdaptor<EmailDeliveryStatusReportAdaptor>();
			return communicationsClientBuilder;
		}
	}
}