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
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;

namespace Transmitly.ChannelProvider.Infobip
{
	//Source = https://stackoverflow.com/a/67857546
	sealed class InfobipDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
	{
		public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			Debug.Assert(typeToConvert == typeof(DateTimeOffset));
			return DateTimeOffset.Parse(reader.GetString(), new CultureInfo("en-US"));
		}

		public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}
