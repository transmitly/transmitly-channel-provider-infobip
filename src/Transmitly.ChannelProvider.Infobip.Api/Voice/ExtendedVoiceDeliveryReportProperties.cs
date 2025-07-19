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
using Transmitly.ChannelProvider.Infobip.Configuration;
using Transmitly.ChannelProvider.Infobip.Configuration.Voice;
using Transmitly.Delivery;
using Transmitly.Util;


namespace Transmitly.ChannelProvider.Infobip.Api.Voice
{
	sealed class ExtendedVoiceDeliveryReportProperties : IVoiceExtendedDeliveryReportProperties
	{
		private readonly IExtendedProperties _extendedProperties;
		private const string ProviderKey = InfobipConstant.SmsPropertiesKey;
		internal ExtendedVoiceDeliveryReportProperties(DeliveryReport deliveryReport, VoiceStatusReport report) : this(deliveryReport)
		{

		}

		internal ExtendedVoiceDeliveryReportProperties(DeliveryReport deliveryReport)
		{
			_extendedProperties = Guard.AgainstNull(deliveryReport).ExtendedProperties;
		}

		internal ExtendedVoiceDeliveryReportProperties(IExtendedProperties properties)
		{
			_extendedProperties = Guard.AgainstNull(properties);
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
		/// Destination address of the voice message.
		/// </summary>
		public string? To
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(To));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(To), value);
		}

		/// <summary>
		/// Date and time when the voice message was initiated. 
		/// Has the following format: yyyy-MM-dd'T'HH:mm:ss.SSSZ.
		/// </summary>
		public string? From
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(From));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(From), value);
		}

		/// <summary>
		/// Mobile country and network codes.
		/// </summary>
		public string? MccMnc
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(MccMnc));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(MccMnc), value);
		}

		/// <summary>
		/// Custom data sent over to the notifyUrl.
		/// </summary>
		public string? CallbackData
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(CallbackData));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(CallbackData), value);
		}

		/// <summary>
		/// Sent Voice message price.
		/// </summary>
		public IVoicePrice? Price
		{
			get => _extendedProperties.GetValue<VoicePrice?>(ProviderKey, nameof(Price));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Price), value);
		}

		/// <summary>
		/// Fields representing details specific for voice messages.
		/// </summary>
		public IVoiceCallDetail? VoiceCall
		{
			get => _extendedProperties.GetValue<VoiceCallDetail?>(ProviderKey, nameof(VoiceCall));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(VoiceCall), value);
		}

		/// <summary>
		/// Indicates whether the message is successfully sent, not sent, delivered, not delivered, waiting for delivery or any other possible status.
		/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">Response status and error codes</see>
		/// </summary>
		public ICallbackStatus? Status
		{
			get => _extendedProperties.GetValue<CallbackStatus?>(InfobipConstant.SmsPropertiesKey, nameof(Status));
			set => _extendedProperties.AddOrUpdate(InfobipConstant.SmsPropertiesKey, nameof(Status), value);
		}

		/// <summary>
		/// Indicates whether the error occurred during the query execution.
		/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">Response status and error codes</see>
		/// </summary>
		public IErrorStatus? Error
		{
			get => _extendedProperties.GetValue<ErrorStatus?>(InfobipConstant.SmsPropertiesKey, nameof(Error));
			set => _extendedProperties.AddOrUpdate(InfobipConstant.SmsPropertiesKey, nameof(Error), value);
		}
	}
}