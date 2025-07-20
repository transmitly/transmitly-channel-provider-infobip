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

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Transmitly.Util;

namespace Transmitly.ChannelProvider.Infobip.Api.Sms.SendSmsMessage
{
	/// <summary>
	/// <see href="https://www.infobip.com/docs/api/channels/sms/sms-messaging/outbound-sms/send-sms-message">Send SMS Message Documentation</see>
	/// </summary>
	internal sealed class SendSmsMessageRequest(List<SendSmsMessageRequestMessage> messages)
	{
		/// <summary>
		/// Unique ID assigned to the request if messaging multiple recipients or sending multiple messages via a single API request. 
		/// If not provided, it will be auto-generated and returned in the API response. 
		/// Typically, used to fetch <see href="https://www.infobip.com/docs/api/channels/sms/sms-messaging/outbound-sms/send-sms-message#channels/sms/get-outbound-sms-message-delivery-reports">delivery reports</see> 
		/// and <see href="https://www.infobip.com/docs/api/channels/sms/sms-messaging/outbound-sms/send-sms-message#channels/sms/get-outbound-sms-message-logs">message logs</see>.
		/// </summary>
		[JsonPropertyName("bulkId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? BulkId { get; set; }

		/// <summary>
		/// An array of message objects of a single message or multiple messages sent under one bulk ID.
		/// </summary>
		[JsonPropertyName("messages")]
		public List<SendSmsMessageRequestMessage> Messages { get; } = Guard.AgainstNullOrEmpty(messages);


	}
}
