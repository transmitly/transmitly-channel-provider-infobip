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

using System.Text.Json.Serialization;

namespace Transmitly.ChannelProvider.Infobip.Voice
{
	public sealed class ExtendedVoiceDeliveryReportProperties
	{
		private readonly IExtendedProperties _extendedProperties;
		private const string ProviderKey = Constant.SmsPropertiesKey;
		internal ExtendedVoiceDeliveryReportProperties(DeliveryReport deliveryReport)
		{
			_extendedProperties = Guard.AgainstNull(deliveryReport).ExtendedProperties;
		}

		internal ExtendedVoiceDeliveryReportProperties(IExtendedProperties properties)
		{
			_extendedProperties = Guard.AgainstNull(properties);
		}

		internal void Apply(VoiceStatusReport report)
		{
			BulkId = report.BulkId;
			MessageId = report.MessageId;
		}

		/// <summary>
		/// Unique ID assigned to the request if messaging multiple recipients 
		/// or sending multiple messages via a single API request.
		/// </summary>
		public string? BulkId
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(BulkId));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(BulkId), value);
		}

		/// <summary>
		/// Unique message ID.
		/// </summary>
		public string? MessageId
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(MessageId));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(MessageId), value);
		}

		/// <summary>
		/// Sent Voice message price.
		/// </summary>
		public VoicePrice? Price
		{
			get => _extendedProperties.GetValue<VoicePrice?>(ProviderKey, nameof(Price));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Price), value);
		}

		/// <summary>
		/// Indicates whether the message is successfully sent, not sent, delivered, not delivered, waiting for delivery or any other possible status.
		/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">Response status and error codes</see>
		/// </summary>
		public CallbackStatus? Status
		{
			get => _extendedProperties.GetValue<CallbackStatus?>(Constant.SmsPropertiesKey, nameof(Status));
			set => _extendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(Status), value);
		}

		/// <summary>
		/// Indicates whether the error occurred during the query execution.
		/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">Response status and error codes</see>
		/// </summary>
		public ErrorStatus? Error
		{
			get => _extendedProperties.GetValue<ErrorStatus?>(Constant.SmsPropertiesKey, nameof(Error));
			set => _extendedProperties.AddOrUpdate(Constant.SmsPropertiesKey, nameof(Error), value);
		}
	}
}