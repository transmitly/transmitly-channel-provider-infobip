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

using System.Net.Http;

namespace Transmitly.Infobip
{
	abstract class BaseClientChannelProviderRestClient(HttpClient? httpClient)
	{
		private static HttpClient? _staticHttpClient;
		private readonly HttpClient? _instanceHttpClient = httpClient;
		private readonly static object _resourceLock = new();

		protected HttpClient GetHttpClient()
		{
			if (_instanceHttpClient != null)
				return _instanceHttpClient;

			if (_staticHttpClient == null)
			{
				lock (_resourceLock)
				{
					if (_staticHttpClient == null)
					{
#pragma warning disable S2696 // Instance members should not write to "static" fields
						_staticHttpClient = new HttpClient();
#pragma warning restore S2696 // Instance members should not write to "static" fields
						ConfigureHttpClient(_staticHttpClient);
					}
				}
			}
			return _staticHttpClient;
		}

		protected virtual void ConfigureHttpClient(HttpClient client)
		{
			Guard.AgainstNull(client);
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}