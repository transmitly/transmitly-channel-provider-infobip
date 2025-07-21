# Transmitly.ChannelProvider.Infobip

A [Transmitly](https://github.com/transmitly/transmitly) channel provider that enables sending Email and SMS communications with [Infobip](https://www.infobip.com/).

### Getting started

To use the Infobip channel provider, first install the [NuGet package](https://nuget.org/packages/transmitly.channelprovider.infobip):

```shell
dotnet add package Transmitly.ChannelProvider.Infobip
```

Then add the channel provider using `AddInfobipSupport()`:

```csharp
using Transmitly;
...
var communicationClient = new CommunicationsClientBuilder()
.AddInfobipSupport(options =>
{
	options.BasePath = "https://base.infobip.com";
	options.ApiKey = "key";
	options.ApiKeyPrefix = "App";
})
.AddPipeline("first-pipeline", pipeline =>
{
	//AddEmail is a channel that is core to the Transmitly library.
	//AsIdentityAddress() is also a convenience method that helps us create an identity address
	//Identity addresses can be anything, email, phone, or even a device/app Id for push notifications!
	pipeline.AddEmail("from@mydomain.com".AsIdentityAddress("Test Display Name"), email =>
	{
		//Transmitly is a bit different. All of our content is supported by templates out of the box.
		//There are multiple types of templates to get you started. You can even create templates 
		//specific to certain cultures!
		email.Subject.AddStringTemplate("Hey, Check out Transmit.ly!");
		email.HtmlBody.AddStringTemplate("Hey, check out this cool new library for managing app communications. <a href=\"https://transmit.ly\">");
		email.TextBody.AddStringTemplate("Hey, check out this cool new library. https://transmitly.ly");
	});

	//AddSms is a channel that is core to the Transmitly library.
	pipeline.AddSms(sms =>
	{
		sms.Message.AddStringTemplate("Check out Transmit.ly!");
	});
});
//Dispatch (send) the transsactional messages to our friend Joe (joe@mydomain.com & 888-555-1234) using our configured InfoBip account with our "first-pipeline" pipeline.
var result = await communicationsClient.DispatchAsync("first-pipeline", ["joe@mydomain.com".AsIdentityAddress("Joe"),"+18885551234".AsIdentityAddress()], new { });
```
* See the [Transmitly](https://github.com/transmitly/transmitly) project for more details on what a channel provider is and how it can be configured.


<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://github.com/transmitly/transmitly/assets/3877248/524f26c8-f670-4dfa-be78-badda0f48bfb">
  <img alt="an open-source project sponsored by CiLabs of Code Impressions, LLC" src="https://github.com/transmitly/transmitly/assets/3877248/34239edd-234d-4bee-9352-49d781716364" width="350" align="right">
</picture> 

---------------------------------------------------

_Copyright &copy; Code Impressions, LLC - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
