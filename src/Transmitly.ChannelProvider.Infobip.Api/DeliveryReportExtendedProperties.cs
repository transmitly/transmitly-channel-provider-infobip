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

using Transmitly.ChannelProvider.Infobip.Api.Sms;
using Transmitly.ChannelProvider.Infobip.Api.Voice;
using Transmitly.ChannelProvider.Infobip.Configuration;
using Transmitly.ChannelProvider.Infobip.Configuration.Sms;
using Transmitly.ChannelProvider.Infobip.Configuration.Voice;
using Transmitly.Delivery;

namespace Transmitly.ChannelProvider.Infobip.Api
{
	public sealed class DeliveryReportExtendedProperties(DeliveryReport deliveryReport)
	{
		public ISmsExtendedDeliveryReportProperties Sms { get; } = new ExtendedSmsDeliveryReportProperties(deliveryReport);
		public IVoiceExtendedDeliveryReportProperties Voice { get; } = new ExtendedVoiceDeliveryReportProperties(deliveryReport);

		/// <summary>
		/// Indicates whether the message is successfully sent, not sent, delivered, not delivered, waiting for delivery or any other possible status.
		/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">Response status and error codes</see>
		/// </summary>
		public ICallbackStatus? Status
		{
			get => Sms.Status ?? Voice.Status;
		}

		/// <summary>
		/// Indicates whether the error occurred during the query execution.
		/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">Response status and error codes</see>
		/// </summary>
		public IErrorStatus? Error
		{
			get => Sms.Error ?? Voice.Error;
		}
	}
}