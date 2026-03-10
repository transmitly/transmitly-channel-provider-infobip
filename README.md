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
		"customer@example.com".AsIdentityAddress("Customer"),
		"+15551234567".AsIdentityAddress()
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
