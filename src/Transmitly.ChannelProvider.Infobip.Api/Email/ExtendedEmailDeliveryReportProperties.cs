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
using Transmitly.ChannelProvider.Infobip.Configuration;
using Transmitly.Delivery;
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Email
{
	public sealed class ExtendedEmailDeliveryReportProperties
	{
		private readonly IExtendedProperties _extendedProperties;
		private const string ProviderKey = InfobipConstant.SmsPropertiesKey;
		internal ExtendedEmailDeliveryReportProperties(DeliveryReport deliveryReport)
		{
			_extendedProperties = Guard.AgainstNull(deliveryReport).ExtendedProperties;
		}

		internal ExtendedEmailDeliveryReportProperties(IExtendedProperties properties)
		{
			_extendedProperties = Guard.AgainstNull(properties);
		}

		internal void Apply(EmailStatusReport report)
		{
			BulkId = report.BulkId;
			MessageId = report.MessageId;
			To = report.To;
			SentAt = report.SentAt;
			DoneAt = report.DoneAt;
			SmsCount = report.SmsCount;
			CallbackData = report.CallbackData;
			Price = report.Price;
			Status = report.Status;
			Error = report.Error;
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
		/// Message destination address.
		/// </summary>
		public string? To
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(To));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(To), value);
		}

		/// <summary>
		/// Date and time when the message was scheduled to be sent. 
		/// Has the following format: yyyy-MM-dd'T'HH:mm:ss.SSSZ.
		/// </summary>
		public DateTimeOffset? SentAt
		{
			get => _extendedProperties.GetValue<DateTimeOffset?>(ProviderKey, nameof(SentAt));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(SentAt), value);
		}

		/// <summary>
		/// Date and time when the Infobip services finished processing the message 
		/// (i.e., delivered to the destination, delivered to the destination network, etc.). 
		/// Has the following format: yyyy-MM-dd'T'HH:mm:ss.SSSZ.
		/// </summary>
		public DateTimeOffset? DoneAt
		{
			get => _extendedProperties.GetValue<DateTimeOffset?>(ProviderKey, nameof(DoneAt));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(DoneAt), value);
		}

		/// <summary>
		/// The number of emails sent.
		/// </summary>
		public int? SmsCount
		{
			get => _extendedProperties.GetValue<int?>(ProviderKey, nameof(SmsCount));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(SmsCount), value);
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
		/// Sent Email price.
		/// </summary>
		public EmailPrice? Price
		{
			get => _extendedProperties.GetValue<EmailPrice?>(ProviderKey, nameof(Price));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Price), value);
		}
		/// <summary>
		/// Indicates the status of the message and how to recover from an error should there be any.
		/// </summary>
		public ICallbackStatus? Status
		{
			get => _extendedProperties.GetValue<CallbackStatus?>(ProviderKey, nameof(Status));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Status), value);
		}

		/// <summary>
		/// Indicates whether an error occurred during the query execution.
		/// </summary>
		public IErrorStatus? Error
		{
			get => _extendedProperties.GetValue<ErrorStatus?>(ProviderKey, nameof(Error));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Error), value);
		}
	}
}