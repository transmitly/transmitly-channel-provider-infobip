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
using Transmitly.Delivery;

namespace Transmitly.ChannelProvider.Infobip.Voice
{
	sealed record VoiceDeliveryReport : DeliveryReport, IVoiceDeliveryReport
	{
		public VoiceDeliveryReport(DeliveryReport original) : base(original)
		{
		}

		public VoiceDeliveryReport(string EventName, string? ChannelId, string? ChannelProviderId, string? PipelineName,
				string? ResourceId, DispatchStatus DispatchStatus, object? ChannelCommunication, IContentModel? ContentModel)
			: base(EventName, ChannelId, ChannelProviderId, PipelineName, ResourceId, DispatchStatus, ChannelCommunication, ContentModel)
		{

		}

		public string? To => this.Infobip().Voice.To;

		public string? From => this.Infobip().Voice.From;

		public TimeSpan Duration => TimeSpan.FromSeconds(this.Infobip().Voice?.VoiceCall?.Duration ?? 0);
	}
}