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
using System.Text.Json;
using System.Threading.Tasks;
using Transmitly.Delivery;

namespace Transmitly.ChannelProvider.Infobip.Voice
{
	sealed class VoiceDeliveryStatusReportAdaptor : IChannelProviderDeliveryReportRequestAdaptor
	{
		public Task<IReadOnlyCollection<DeliveryReport>?> AdaptAsync(IRequestAdaptorContext adaptorContext)
		{
			if (!ShouldAdapt(adaptorContext.Content))
				return Task.FromResult<IReadOnlyCollection<DeliveryReport>?>(null);

			var statuses = JsonSerializer.Deserialize<VoiceStatusReports>(adaptorContext.Content!);

			if (statuses?.Results == null)
				return Task.FromResult<IReadOnlyCollection<DeliveryReport>?>(null);

			var ret = new List<DeliveryReport>(statuses.Results.Count);
			foreach (var voiceReport in statuses.Results)
			{
				var report = new VoiceDeliveryReport(
						DeliveryReport.Event.StatusChanged(),
						Id.Channel.Voice(),
						Id.ChannelProvider.Infobip(),
						adaptorContext.PipelineName,
						voiceReport.MessageId,
						Util.ToDispatchStatus(voiceReport.Status?.GroupId),
						null,
						null, 
						null
					)
					.ApplyExtendedProperties(voiceReport);
				
				ret.Add(report);
			}

			return Task.FromResult<IReadOnlyCollection<DeliveryReport>?>(ret);
		}

		private static bool ShouldAdapt(string? content)
		{
			if (string.IsNullOrWhiteSpace(content))
				return false;
			return Array.TrueForAll(["voiceCall", "pricePerSecond", "answerTime", "messageId", "bulkId"], x => content!.Contains(x));
		}
	}
}