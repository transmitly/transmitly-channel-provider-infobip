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

namespace Transmitly.ChannelProvider.Infobip.Api.Sms
{
	internal static class ExtendedSmsDeliveryReportExtension
	{
		public static SmsDeliveryReport ApplyExtendedProperties(this SmsDeliveryReport voiceDeliveryReport, SmsStatusReport report)
		{
			_ = new ExtendedSmsDeliveryReportProperties(voiceDeliveryReport)
			{
				BulkId = report.BulkId,
				MessageId = report.MessageId,
				To = report.To,
				From = report.From,
				MccMnc = report.MccMnc,
				CallbackData = report.CallbackData,
				Price = report.Price,
				ApplicationId = report.ApplicationId,
				EntityId = report.EntityId,
				Status = report.Status,
				Error = report.Error
			};
			return voiceDeliveryReport;
		}
	}
}