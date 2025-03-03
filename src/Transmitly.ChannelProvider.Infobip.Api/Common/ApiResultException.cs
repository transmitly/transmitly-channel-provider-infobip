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

namespace Transmitly.ChannelProvider.Infobip
{
	public sealed class ApiResultException : Exception
	{
		public string? MessageId { get; }
		public string? Text { get; }
		public IReadOnlyCollection<string>? ValidationErrors { get; }
		internal ApiResultException(ApiRequestErrorResult? requestError) : base(string.Join(",", requestError?.ServiceException?.ValidationErrors ?? []))
		{
			if (requestError == null || requestError.ServiceException == null)
				return;
			MessageId = requestError.ServiceException.MessageId;
			Text = requestError.ServiceException.Text;
			ValidationErrors = requestError.ServiceException.ValidationErrors?.AsReadOnly();
		}
	}
}