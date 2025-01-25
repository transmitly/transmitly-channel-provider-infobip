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
using System.Threading.Tasks;
using Transmitly.ChannelProvider.Infobip.Configuration.Sms;

namespace Transmitly.ChannelProvider.Infobip.Sms
{
    public sealed class SmsExtendedChannelProperties : ISmsExtendedChannelProperties
    {
        private readonly IExtendedProperties _extendedProperties;
        private const string ProviderKey = Constant.SmsPropertiesKey;

        internal SmsExtendedChannelProperties(ISmsChannel smsChannel)
        {
            Guard.AgainstNull(smsChannel);
            _extendedProperties = Guard.AgainstNull(smsChannel.ExtendedProperties);

        }

        internal SmsExtendedChannelProperties(IExtendedProperties properties)
        {
            _extendedProperties = properties;
        }

        public SmsExtendedChannelProperties()
        {
            
        }

        /// <summary>
        /// The message validity period in minutes. When the period expires, it will not be allowed for the message to be sent. 
        /// Validity period longer than 48h is not supported. 
        /// Any bigger value will automatically default back to 2880
        /// </summary>
        public long? ValidityPeriod
        {
            get => _extendedProperties.GetValue<long?>(ProviderKey, nameof(ValidityPeriod));
            set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(ValidityPeriod), value);
        }

        /// <summary>
        /// Required for entity use in a send request for outbound traffic. 
        /// Returned in notification events. For more details, see our 
        /// <see href="https://www.infobip.com/docs/cpaas-x/application-and-entity-management">documentation</see>.
        /// </summary>
        public string? EntityId
        {
            get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(EntityId));
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value!.Length > 50)
                    throw new InfobipException(nameof(EntityId) + " cannot be greater than 50 characters");

                _extendedProperties.AddOrUpdate(ProviderKey, nameof(EntityId), value);
            }
        }

        /// <summary>
        /// Required for application use in a send request for outbound traffic. 
        /// Returned in notification events. For more details, see our 
        /// <see href="https://www.infobip.com/docs/cpaas-x/application-and-entity-management">documentation</see>
        /// </summary>
        public string? ApplicationId
        {
            get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(ApplicationId));
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value!.Length > 50)
                    throw new InfobipException(nameof(ApplicationId) + " cannot be greater than 50 characters");

                _extendedProperties.AddOrUpdate(ProviderKey, nameof(ApplicationId), value);
            }
        }

        /// <summary>
        /// The URL on your call back server on to which a delivery report will be sent. 
        /// The retry cycle for when your URL becomes unavailable uses the following formula: 1min + (1min * retryNumber * retryNumber).
        /// <para><see href="https://www.infobip.com/docs/api/channels/sms/sms-messaging/outbound-sms/send-sms-message#channels/sms/receive-sent-sms-report">Delivery report format</see></para>
        ///  <para>The communication's ResourceId will be added to the querystring.
        /// Example: https://yourUrl.com/path?<strong>resourceId=1234abc</strong></para>
        /// <para>If you require dynamic resolution see: <see cref="NotifyUrlResolver"/>. Setting the <see cref="NotifyUrlResolver"/> will override this value.</para>
        /// </summary>
        public string? NotifyUrl
        {
            get => _extendedProperties.GetValue<string?>(ProviderKey, nameof(NotifyUrl));
            set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(NotifyUrl), value);
        }

        /// <summary>
        /// The resolver that will return the URL on your call back server on to which a delivery report will be sent. 
        /// The retry cycle for when your URL becomes unavailable uses the following formula: 1min + (1min * retryNumber * retryNumber).
        /// <para><see href="https://www.infobip.com/docs/api/channels/sms/sms-messaging/outbound-sms/send-sms-message#channels/sms/receive-sent-sms-report">Delivery report format</see></para>
        /// <para>This will override any value set in the <see cref="NotifyUrl"/> property.</para>
        /// </summary>
        public Func<IDispatchCommunicationContext, Task<string?>>? NotifyUrlResolver
        {
            get => _extendedProperties.GetValue<Func<IDispatchCommunicationContext, Task<string?>>>(ProviderKey, nameof(NotifyUrlResolver));
            set => _extendedProperties.AddOrUpdate(ProviderKey, nameof(NotifyUrlResolver), value);
        }

        public ISmsExtendedChannelProperties Adapt(ISmsChannel sms)
        {
            return new SmsExtendedChannelProperties(sms);
        }
    }
}