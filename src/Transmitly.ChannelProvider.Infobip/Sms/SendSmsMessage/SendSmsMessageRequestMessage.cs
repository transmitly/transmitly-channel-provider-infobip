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

namespace Transmitly.ChannelProvider.Infobip.Sms.SendSmsMessage
{
	internal sealed class SendSmsMessageRequestMessage(string? text, List<SendSmsMessageRequestMessageDestination> destinations)
	{
		/// <summary>
		/// Additional data that can be used for identifying, managing, or monitoring a message. 
		/// Data included here will also be automatically included in the message Delivery Report. 
		/// The maximum value is 4000 characters and any overhead may be truncated
		/// </summary>
		[JsonPropertyName("callbackData")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? CallbackData { get; set; }

		/// <summary>
		/// An array of destination objects for where messages are being sent. A valid destination is required.
		/// </summary>
		[JsonPropertyName("destinations")]
		public List<SendSmsMessageRequestMessageDestination> Destinations { get; set; } = Guard.AgainstNullOrEmpty(destinations);

		/// <summary>
		/// Content of the message being sent.
		/// </summary>
		[JsonPropertyName("text")]
		public string Text { get; set; } = Guard.AgainstNullOrWhiteSpace(text);

		/// <summary>
		/// The message validity period in minutes. When the period expires, it will not be allowed for the message to be sent. 
		/// Validity period longer than 48h is not supported. Any bigger value will automatically default back to 2880.
		/// </summary>
		[JsonPropertyName("validityPeriod")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? ValidityPeriod { get; set; }

		/// <summary>
		/// Required for application use in a send request for outbound traffic. 
		/// Returned in notification events. For more details, see our 
		/// <see href="https://www.infobip.com/docs/cpaas-x/application-and-entity-management">documentation</see>
		/// </summary>
		[JsonPropertyName("applicationId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? ApplicationId { get; set; }

		/// <summary>
		/// Required for entity use in a send request for outbound traffic. 
		/// Returned in notification events. For more details, see our 
		/// <see href="https://www.infobip.com/docs/cpaas-x/application-and-entity-management">documentation</see>.
		/// </summary>
		[JsonPropertyName("entityId")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? EntityId { get; set; }

		/// <summary>
		/// The URL on your call back server on to which a delivery report will be sent.
		/// </summary>
		[JsonPropertyName("notifyUrl")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? NotifyUrl { get; set; }
	}
}
