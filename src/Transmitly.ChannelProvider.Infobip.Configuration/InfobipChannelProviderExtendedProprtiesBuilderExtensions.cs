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
using Transmitly.ChannelProvider.Configuration;
using Transmitly.ChannelProvider.Infobip.Configuration.Email;
using Transmitly.ChannelProvider.Infobip.Configuration.Sms;
using Transmitly.ChannelProvider.Infobip.Configuration.Voice;
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Configuration
{
	public static class InfobipChannelProviderExtendedProprtiesBuilderExtensions
	{
		private static Type? _smsAdaptorType;
		internal static ISmsExtendedChannelProperties Sms => Create<ISmsExtendedChannelProperties>(Guard.AgainstNull(_smsAdaptorType));

		private static Type? _voiceAdaptorType;
		internal static IVoiceExtendedChannelProperties Voice => Create<IVoiceExtendedChannelProperties>(Guard.AgainstNull(_voiceAdaptorType));

		private static Type? _emailAdaptorType;
		internal static IEmailExtendedChannelProperties Email => Create<IEmailExtendedChannelProperties>(Guard.AgainstNull(_emailAdaptorType));


		private static Type? _deliveryReportAdaptorType;
		internal static IDeliveryReportExtendedProperties DeliveryReport => Create<IDeliveryReportExtendedProperties>(Guard.AgainstNull(_deliveryReportAdaptorType));

		private static T Create<T>(Type t)
		{
			return (T)Guard.AgainstNull(Activator.CreateInstance(t));
		}

		public static ChannelProviderRegistrationBuilder AddSmsExtendedPropertiesAdaptor<T>(this ChannelProviderRegistrationBuilder builder)
			where T : class, ISmsExtendedChannelProperties, new()
		{
			_smsAdaptorType = typeof(T);
			return builder;
		}

		public static ChannelProviderRegistrationBuilder AddVoiceExtendedPropertiesAdaptor<T>(this ChannelProviderRegistrationBuilder builder)
			where T : class, IVoiceExtendedChannelProperties, new()
		{
			_voiceAdaptorType = typeof(T);
			return builder;
		}


		public static ChannelProviderRegistrationBuilder AddEmailExtendedPropertiesAdaptor<T>(this ChannelProviderRegistrationBuilder builder)
			where T : class, IEmailExtendedChannelProperties, new()
		{
			_emailAdaptorType = typeof(T);
			return builder;
		}

		public static ChannelProviderRegistrationBuilder AddDeliveryReportExtendedProprtiesAdaptor<T>(this ChannelProviderRegistrationBuilder builder)
			where T : class, IDeliveryReportExtendedProperties, new()
		{
			_deliveryReportAdaptorType = typeof(T);
			return builder;
		}
	}
}