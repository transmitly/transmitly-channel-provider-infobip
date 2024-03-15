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

using IB = Infobip.Api.Client;
using IBApi = Infobip.Api.Client.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transmitly.ChannelProvider;
using Infobip.Api.Client.Model;
using Transmitly.Exceptions;

namespace Transmitly.Infobip
{
	internal sealed class InfobipSmsChannelProviderClient(IB.Configuration optionObj) : ChannelProviderClient<ISms>
	{
		private readonly IB.Configuration _optionObj = Guard.AgainstNull(optionObj);

		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

		public override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(ISms sms, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
		{
			Guard.AgainstNull(sms);
			Guard.AgainstNull(communicationContext);
			var messageId = Guid.NewGuid().ToString("N");
			var apiClient = new IBApi.SendSmsApi(_optionObj);
			var to = sms.To?.Select(s => new SmsDestination(messageId, s.Value)).ToList();
			//var attachment = email.Attachments.FirstOrDefault()?.ContentStream;
			//var from = email.From.ToEmailAddress();
			var sendResult = await apiClient.SendSmsMessageAsync(new IB.Model.SmsAdvancedTextualRequest(null, [new SmsTextualMessage(from: sms.From?.Value, destinations: to, text: sms.Body)]), cancellationToken);

			if (sendResult.Messages.Count != 1)
			{
				Error(communicationContext, sms);
				throw new CommunicationsException("Unexpected # of messages");
			}

			var message = sendResult.Messages[0];
			Dispatched(communicationContext, sms);
			return [new InfobipDispatchResult
			{
				ResourceId = message.MessageId,
				DispatchStatus = DispatchStatus.Dispatched,
			}];
		}
	}
}