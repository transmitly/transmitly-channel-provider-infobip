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

using Transmitly.ChannelProvider.Infobip.Configuration.Email;
using Transmitly.ChannelProvider.Infobip.Configuration.Sms;
using Transmitly.ChannelProvider.Infobip.Configuration.Voice;
using Transmitly.Delivery;

namespace Transmitly.ChannelProvider.Infobip.Configuration
{
	public static class InfobipChannelProviderConfigurationExtensions
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
		/// <param name="email">Email Channel.</param>
		/// <returns>Infobip email properties.</returns>
		public static IEmailExtendedChannelProperties Infobip(this IEmailChannel email)
		{
			return InfobipChannelProviderExtendedProprtiesBuilderExtensions.Email.Adapt(email);
		}

		/// <summary>
		/// Infobip specific settings for Sms channels.
		/// </summary>
		/// <param name="sms">Sms Channel.</param>
		/// <returns>Infobip Sms properties.</returns>
		public static ISmsExtendedChannelProperties Infobip(this ISmsChannel sms)
		{
			return InfobipChannelProviderExtendedProprtiesBuilderExtensions.Sms.Adapt(sms);
		}

		/// <summary>
		/// Infobip specific settings for voice channels.
		/// </summary>
		/// <param name="voice">Voice Channel.</param>
		/// <returns>Infobip voice properties.</returns>
		public static IVoiceExtendedChannelProperties Infobip(this IVoiceChannel voice)
		{
			return InfobipChannelProviderExtendedProprtiesBuilderExtensions.Voice.Adapt(voice);
		}

		/// <summary>
		/// Infobip specific settings for sms delivery reports.
		/// </summary>
		/// <param name="deliveryReport">Delivery Report.</param>
		/// <returns>Infobip SMS delivery report properties.</returns>
		public static IDeliveryReportExtendedProperties Infobip(this DeliveryReport deliveryReport)
		{
			return InfobipChannelProviderExtendedProprtiesBuilderExtensions.DeliveryReport.Adapt(deliveryReport);
		}

	}
}