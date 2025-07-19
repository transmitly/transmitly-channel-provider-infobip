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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Transmitly.ChannelProvider.Infobip.Api
{
	sealed class JsonEnumMemberAttributeConverter<TEnum> : JsonStringEnumConverter
		where TEnum : struct, Enum
	{
		public JsonEnumMemberAttributeConverter() : base(namingPolicy: ResolveNamingPolicy())
		{
		}

		private static JsonNamingPolicy? ResolveNamingPolicy()
		{
			var map = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(f => (f.Name, AttributeName: f.GetCustomAttribute<EnumMemberAttribute>()?.Value))
				.Where(pair => pair.AttributeName != null)
				.ToDictionary(k => k.Name, v => v.AttributeName);

			return map.Count > 0 ? new EnumMemberNamingPolicy(map!) : null;
		}

		private sealed class EnumMemberNamingPolicy(Dictionary<string, string> map) : JsonNamingPolicy
		{
			public override string ConvertName(string name)
				=> map.TryGetValue(name, out string? newName) ? newName : name;
		}
	}
}
