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
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Transmitly.ChannelProvider.Infobip.Sms
{
	sealed class SmsDeliveryStatusReportAdaptor : IChannelProviderDeliveryReportRequestAdaptor
	{
		public Task<IReadOnlyCollection<DeliveryReport>?> AdaptAsync(string requestBody)
		{
			if (!ShouldAdapt(requestBody))
				return Task.FromResult<IReadOnlyCollection<DeliveryReport>?>(null);

			var statuses = JsonSerializer.Deserialize<SmsStatusReports>(requestBody);

			if (statuses?.Results == null)
				return Task.FromResult<IReadOnlyCollection<DeliveryReport>?>(null);

			var ret = new List<DeliveryReport>(statuses.Results.Count);
			foreach (var smsReport in statuses.Results)
			{
				var report = new DeliveryReport(
						DeliveryReport.Event.StatusChanged(),
						Id.Channel.Sms(),
						Id.ChannelProvider.Infobip(),
						null,
						smsReport.MessageId,
						Util.ToDispatchStatus(smsReport.Status?.GroupId),
						null
					);

				new ExtendedSmsDeliveryReportProperties(report).Apply(smsReport);
				ret.Add(report);
			}

			return Task.FromResult<IReadOnlyCollection<DeliveryReport>?>(ret);
		}

		private static bool ShouldAdapt(string requestBody)
		{
			return Array.TrueForAll(["pricePerMessage", "smsCount", "messageId", "bulkId"], x => requestBody.Contains(x));
		}
	}
}