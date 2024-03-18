//// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
////  
////  Licensed under the Apache License, Version 2.0 (the "License")
////  you may not use this file except in compliance with the License.
////  You may obtain a copy of the License at
////  
////      http://www.apache.org/licenses/LICENSE-2.0
////  
////  Unless required by applicable law or agreed to in writing, software
////  distributed under the License is distributed on an "AS IS" BASIS,
////  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////  See the License for the specific language governing permissions and
////  limitations under the License.

//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Transmitly.ChannelProvider.Infobip.Email
//{
//	internal sealed class EmailChannelProviderClient(InfobipChannelProviderConfiguration optionObj) : ChannelProviderClient<IEmail>
//	{
//		private readonly InfobipChannelProviderConfiguration _optionObj = Guard.AgainstNull(optionObj);

//		public override IReadOnlyCollection<string>? RegisteredEvents => [DeliveryReportEvent.Name.Dispatched(), DeliveryReportEvent.Name.Error()];

//		public override async Task<IReadOnlyCollection<IDispatchResult?>> DispatchAsync(IEmail email, IDispatchCommunicationContext communicationContext, CancellationToken cancellationToken)
//		{
//			throw new NotImplementedException();
//			//Guard.AgainstNull(email);
//			//Guard.AgainstNull(communicationContext);
//			//var messageId = Guid.NewGuid().ToString("N");
//			//var apiClient = new IBApi.SendEmailApi(_optionObj);
//			//var to = email.To?.FirstOrDefault()?.ToEmailAddress();
//			//var cc = string.Join(";", email.Cc?.Concat(email.To?.Skip(1) ?? []) ?? []);
//			//var bcc = string.Join(";", email.Bcc?.ToList() ?? []);
//			//var attachment = email.Attachments.FirstOrDefault()?.ContentStream;
//			//var from = email.From.ToEmailAddress();

//			//var sendResult = await apiClient.SendEmailAsync(from, to, email.Subject, cc, bcc, email.TextBody, null, messageId, null, attachment, null, email.HtmlBody, from, cancellationToken: cancellationToken);

//			//if (email.ExtendedProperties.TryGetValue<long?>(Constant.Id, nameof(ExtendedEmailProperties.TemplateId), out var templateId))
//			//    sendResult = await apiClient.SendEmailAsync(from, to, email.Subject, cc, bcc, templateid: templateId);

//			//if (sendResult.Messages.Count != 1)
//			//{
//			//    Error(communicationContext, email);
//			//    throw new CommunicationsException("Unexpected # of messages");
//			//}

//			//var message = sendResult.Messages[0];
//			//Dispatched(communicationContext, email);
//			//return [new InfobipDispatchResult
//			//{
//			//    ResourceId = message.MessageId,
//			//    DispatchStatus = DispatchStatus.Dispatched,
//			//}];
//		}
//	}
//}