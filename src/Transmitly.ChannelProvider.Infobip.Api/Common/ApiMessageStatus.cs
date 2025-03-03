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

namespace Transmitly.ChannelProvider.Infobip
{
	/// <summary>
	/// Indicates whether the message is successfully sent, not sent, delivered, 
	/// not delivered, waiting for delivery or any other possible status.
	/// <see href="https://dev.infobip.com/getting-started/response-status-and-error-codes#status-object-example">
	/// Response status and error codes
	/// </see>
	/// </summary>
	sealed class ApiMessageStatus
	{
		/// <summary>
		/// Status group ID.
		/// </summary>
		[JsonPropertyName("groupId")]
		public int GroupId { get; set; }
		/// <summary>
		/// Status group name that describes which category the status code belongs to, e.g. 
		/// PENDING, UNDELIVERABLE, DELIVERED, EXPIRED, REJECTED.
		/// </summary>
		[JsonPropertyName("groupName")]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public InfobipGroupName GroupName { get; set; }
		/// <summary>
		/// Status ID.
		/// </summary>
		[JsonPropertyName("id")]
		public int Id { get; set; }
		/// <summary>
		/// Status name.
		/// </summary>
		[JsonPropertyName("name")]
		public string? Name { get; set; }
		/// <summary>
		/// Human-readable description of the status.
		/// </summary>
		[JsonPropertyName("description")]
		public string? Description { get; set; }
	}
}