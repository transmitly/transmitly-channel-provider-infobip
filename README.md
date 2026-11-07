# Transmitly.ChannelProvider.Infobip

`Transmitly.ChannelProvider.Infobip` is the convenience package for using [Transmitly](https://github.com/transmitly/transmitly) with [Infobip](https://www.infobip.com/).

This is the package most applications should install. It wires together:

- `Transmitly.ChannelProvider.Infobip.Configuration`
- `Transmitly.ChannelProvider.Infobip.Api`

Supported channels:

- `Email`
- `Sms`
- `Voice`

## Install

```shell
dotnet add package Transmitly.ChannelProvider.Infobip
```

## Quick Start

```csharp
using Transmitly;

ICommunicationsClient client = new CommunicationsClientBuilder()
	.AddInfobipSupport(options =>
	{
		options.BasePath = "https://base.infobip.com";
		options.ApiKey = "your-infobip-api-key";
		options.ApiKeyPrefix = "App";
		// Required unless ValidateDeliveryReportSignature = false. See Configuration.
		options.WebhookSecret = "your-notification-profile-hmac-secret";
	})
	.AddPipeline("account-alert", pipeline =>
	{
		pipeline.AddEmail("alerts@example.com".AsIdentityAddress("Example App"), email =>
		{
			email.Subject.AddStringTemplate("Important account alert");
			email.TextBody.AddStringTemplate("There is an update on your account.");
		});

		pipeline.AddSms(sms =>
		{
			sms.Message.AddStringTemplate("There is an update on your account.");
		});
	})
	.BuildClient();

var result = await client.DispatchAsync(
	"account-alert",
	new[]
	{
		"customer@example.com",
		"+15551234567"
	},
	new { });
```

## Configuration

`AddInfobipSupport(options => ...)` accepts `InfobipChannelProviderConfiguration`.

Common settings:

- `ApiKey`: your Infobip API key.
- `BasePath`: defaults to `https://base.infobip.com`.
- `ApiKeyPrefix`: defaults to `App`.
- `WebProxy`: optional outbound proxy.
- `WebhookSecret`: the secret for delivery-report signatures. In Infobip, create a notification profile for your delivery-report URL with an HMAC authentication setting (algorithm `HMAC_SHA_256`), then use its secret key here. Reports without a valid `X-Hub-Signature` are rejected with `403 Forbidden` by the MVC integration packages. `AddInfobipSupport` throws at startup if it is missing while `ValidateDeliveryReportSignature` is `true`. Only HMAC authentication is supported; Basic, OAuth and mTLS are not yet.
- `ValidateDeliveryReportSignature`: defaults to `true`. Set it to `false` if you don't receive Infobip delivery reports; then no secret is needed.

## Infobip-Specific Channel Features

Infobip channel extensions are available through the configuration package:

- `email.Infobip()` for email-specific settings such as `TemplateId`, tracking flags, `AmpHtml`, and notify URLs.
- `sms.Infobip()` for SMS-specific settings such as `ValidityPeriod` and notify URLs.
- `voice.Infobip()` for voice-specific settings such as timeouts, recording, machine detection, and voice selection.

## Related Packages

- [Transmitly](https://github.com/transmitly/transmitly)
- [Transmitly.ChannelProvider.Infobip.Configuration](https://github.com/transmitly/transmitly-channel-provider-infobip-configuration)
- [Transmitly.ChannelProvider.Infobip.Api](https://github.com/transmitly/transmitly-channel-provider-infobip-api)

---
_Copyright (c) Code Impressions, LLC. This open-source project is sponsored and maintained by Code Impressions and is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
