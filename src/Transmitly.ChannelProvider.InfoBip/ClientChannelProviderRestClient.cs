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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transmitly.ChannelProvider;
using System.Net.Http;
using Infobip.Api.Client;

namespace Transmitly.Infobip
{
	abstract class ClientChannelProviderRestClient<TCommunication>(Configuration configuration, HttpClient? httpClient) : BaseClientChannelProviderRestClient(httpClient), IChannelProviderClient<TCommunication>
	{

		protected HttpClient HttpClient => GetHttpClient();

		public IReadOnlyCollection<string>? RegisteredEvents => throw new NotImplementedException();

		IReadOnlyCollection<string>? IChannelProviderClient.RegisteredEvents => throw new NotImplementedException();

		protected ClientChannelProviderRestClient(Configuration configuration) : this(configuration, null)
		{

		}

		protected abstract Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, TCommunication communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken);
		protected virtual Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(HttpClient restClient, object communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			if (communication is not TCommunication)
				throw new NotSupportedException();
			return DispatchAsync(restClient, (TCommunication)communication, communicationContext, cancellationToken);
		}

		Task<IReadOnlyCollection<IDispatchResult?>> IChannelProviderClient<TCommunication>.DispatchAsync(TCommunication communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			return DispatchAsync(GetHttpClient(), communication, communicationContext, cancellationToken);
		}

		Task<IReadOnlyCollection<IDispatchResult?>> IChannelProviderClient.DispatchAsync(object communication, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			return DispatchAsync(GetHttpClient(), communication, communicationContext, cancellationToken);
		}
	}
}