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
using Transmitly.Channel.Configuration;
using Infobip.Api.Client;
using Transmitly.Infobip;
namespace Transmitly
{
	public static class InfobipChannelProviderExtensions
	{
		private const string InfobipId = "Infobip";
		public static string Infobip(this ChannelProviders channelProviders, params string?[] providerId)
		{
			Guard.AgainstNull(channelProviders);
			if (providerId == null || providerId.Length == 0)
				providerId = ["Default"];
			return $"{InfobipId}.{string.Join(".", providerId)}";
		}

		public static CommunicationsClientBuilder AddInfobipSupport(this ChannelProviderConfigurationBuilder channelProviderConfiguration, Action<Configuration> options, string? providerId = null)
		{
			var optionObj = new Configuration();
			options(optionObj);
			channelProviderConfiguration.Add(Id.ChannelProvider.Infobip(providerId), new InfobipSmsChannelProviderClient(optionObj), Id.Channel.Sms());
			return channelProviderConfiguration.Add(Id.ChannelProvider.Infobip(providerId), new InfobipEmailChannelProviderClient(optionObj), Id.Channel.Email());
		}

		public static CommunicationsClientBuilder AddInfobipSupport(this CommunicationsClientBuilder communicationsClientBuilder, Action<Configuration> options, string? providerId = null)
		{
			return communicationsClientBuilder.ChannelProvider.AddInfobipSupport(options, providerId);
		}
	}
}