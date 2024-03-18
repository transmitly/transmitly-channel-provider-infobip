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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Transmitly.Infobip;

namespace Transmitly.ChannelProvider.Infobip.Email
{
    internal sealed class EmailChannelProviderRestClient(InfobipChannelProviderConfiguration configuration, HttpClient? httpClient) : ChannelProviderRestClient<IEmail>(httpClient)
	{
		private const string SendEmailPath = "/email/3/send";
		private readonly InfobipChannelProviderConfiguration _configuration = configuration;

		public EmailChannelProviderRestClient(InfobipChannelProviderConfiguration configuration) : this(configuration, null)
		{
			System.Text.Json.JsonSerializer.Serialize(new { });
		}

		protected override void ConfigureHttpClient(HttpClient client)
		{
			RestClientConfiguration.Configure(client, _configuration);
			base.ConfigureHttpClient(client);
		}

		protected override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, IEmail communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			var result = await restClient.PostAsync(SendEmailPath, new StringContent(""), cancellationToken);

			throw new NotImplementedException();
		}
	}
}