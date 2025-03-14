﻿// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
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

using System.Threading.Tasks;
using System;
using Transmitly.Template.Configuration;
using Transmitly.ChannelProvider.Infobip.Configuration.Email;

namespace Transmitly.ChannelProvider.Infobip.Email
{
	public sealed class EmailExtendedChannelProperties : IEmailExtendedChannelProperties
	{
		private const string ProviderKey = Constant.EmailPropertiesKey;
		private readonly IExtendedProperties _extendedProperties;

		internal EmailExtendedChannelProperties(IExtendedProperties properties)
		{
			Guard.AgainstNull(properties);
			_extendedProperties = properties;
			AmpHtml = new ContentTemplateConfiguration();
		}

		internal EmailExtendedChannelProperties(IEmailChannel channel) : this(Guard.AgainstNull(channel).ExtendedProperties)
		{
		}

		public EmailExtendedChannelProperties()
		{

		}
		/// <summary>
		/// Template ID used for generating email content. The template is created over Infobip web interface. 
		/// If templateId is present, then html and text values are ignored.
		/// Note: templateId only supports the value of Broadcast. Content and Flow are not supported.
		/// </summary>
		public long? TemplateId
		{
			get => _extendedProperties.GetValue<long?>(ProviderKey, nameof(TemplateId));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(TemplateId), value);
		}

		/// <summary>
		/// Enable or disable open and click tracking. Passing true will only enable tracking and the statistics would be 
		/// visible in the web interface alone. This can be explicitly overridden by <see cref="TrackClicks"/> and <see cref="TrackOpens"/>
		/// </summary>
		public bool Track
		{
			get => _extendedProperties.GetValue(ProviderKey, nameof(Track), true);
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(Track), value);
		}

		/// <summary>
		/// This parameter enables or disables track open feature.
		/// </summary>
		public bool? TrackOpens
		{
			get => _extendedProperties.GetValue<bool?>(ProviderKey, nameof(TrackOpens));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(TrackOpens), value);
		}

		/// <summary>
		/// This parameter enables or disables track click feature.
		/// </summary>
		public bool? TrackClicks
		{
			get => _extendedProperties.GetValue<bool?>(ProviderKey, nameof(TrackClicks));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(TrackClicks), value);
		}

		/// <summary>
		/// Amp HTML body of the message. If ampHtml is present, html is mandatory. 
		/// Amp HTML is not supported by all the email clients. 
		/// <see href="https://developers.google.com/gmail/ampemail/">Please check this link for configuring Gmail client</see>.
		/// </summary>
		public IContentTemplateConfiguration AmpHtml
		{
			get => _extendedProperties.GetValue<ContentTemplateConfiguration>(ProviderKey, nameof(AmpHtml))!;
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(AmpHtml), value);
		}

		/// <summary>
		/// The real-time Intermediate delivery report that will be sent on your callback server.
		/// </summary>
		public bool? IntermediateReport
		{
			get => _extendedProperties.GetValue<bool?>(ProviderKey, nameof(IntermediateReport));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(IntermediateReport), value);
		}

		/// <summary>
		/// The URL on your callback server on which the Delivery report will be sent.
		/// </summary>
		public string? NotifyUrl
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(NotifyUrl));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(NotifyUrl), value);
		}

		/// <summary>
		/// The resolver to get the URL on your callback server on which the Delivery report will be sent.
		/// </summary>
		public Func<IDispatchCommunicationContext, Task<string?>>? NotifyUrlResolver
		{
			get => _extendedProperties.GetValue<Func<IDispatchCommunicationContext, Task<string?>>>(ProviderKey, nameof(NotifyUrlResolver));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(NotifyUrlResolver), value);
		}

		/// <summary>
		/// Required for application use in a send request for outbound traffic. Returned in notification events.
		/// </summary>
		public string? ApplicationId
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(ApplicationId));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(ApplicationId), value);
		}

		/// <summary>
		/// Required for entity use in a send request for outbound traffic. Returned in notification events.
		/// </summary>
		public string? EntityId
		{
			get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(EntityId));
			set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(EntityId), value);
		}

		public IEmailExtendedChannelProperties Adapt(IEmailChannel email)
		{
			return new EmailExtendedChannelProperties(email);
		}
	}
}