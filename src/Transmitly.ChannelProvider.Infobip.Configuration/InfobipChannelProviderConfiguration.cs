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

using System.Net;
namespace Transmitly.ChannelProvider.Infobip.Configuration
{
	public sealed class InfobipChannelProviderConfiguration
	{
		private readonly string _userAgent = GetUserAgent();
		
		public WebProxy? WebProxy { get; set; }
		public string? ApiKey { get; set; }
		public string? BasePath { get; set; }
		public string ApiKeyPrefix { get; set; } = "App";
		public string UserAgent => _userAgent;

		private static string GetUserAgent()
		{
			string? version = null;
			try
			{
				version = typeof(InfobipChannelProviderConfiguration).Assembly.GetName().Version?.ToString();
			}
			catch
			{
				//eat error

			}
			
			if (string.IsNullOrWhiteSpace(version))
				version = "0.1.0";

			return $"transmitly-infobip/{version}";
		}
	}
}