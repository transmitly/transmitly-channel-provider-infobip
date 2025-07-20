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
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Transmitly.ChannelProvider.Infobip.Api.Email.SendEmail
{
	internal sealed class DateTimeConverter : JsonConverter<DateTime>
	{
		private const string Format = "yyyy-MM-dd'T'HH:mm:fffzzz";

		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var dateString = reader.GetString();
				if (DateTime.TryParseExact(
						dateString,
						Format,
						CultureInfo.InvariantCulture,
						DateTimeStyles.None,
						out var result))
				{
					return result;
				}
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString(Format));
		}
	}
}
