# LINGYUN.Abp.PushPlus

Integration with PushPlus

Implements PushPlus related API documentation, providing PushPlus open capabilities.

For details, see PushPlus documentation: https://www.pushplus.plus/doc/guide/openApi.html

## Module Dependencies

```csharp
[DependsOn(typeof(AbpPushPlusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

* PushPlus                                       PushPlus feature group
* PushPlus.Message.Enable                        Enable PushPlus message channel globally
* PushPlus.Channel.WeChat                        WeChat public account message channel
* PushPlus.Channel.WeChat.Enable                 Enable WeChat public account message channel
* PushPlus.Channel.WeChat.SendLimit              WeChat public account message channel send limit
* PushPlus.Channel.WeChat.SendLimitInterval      WeChat public account message channel limit period (days)
* PushPlus.Channel.WeWork                        WeWork message channel
* PushPlus.Channel.WeWork.Enable                 Enable WeWork message channel
* PushPlus.Channel.WeWork.SendLimit              WeWork message channel send limit
* PushPlus.Channel.WeWork.SendLimitInterval      WeWork message channel limit period (days)
* PushPlus.Channel.Webhook                       Webhook message channel
* PushPlus.Channel.Webhook.Enable                Enable Webhook message channel
* PushPlus.Channel.Webhook.SendLimit             Webhook message channel send limit
* PushPlus.Channel.Webhook.SendLimitInterval     Webhook message channel limit period (days)
* PushPlus.Channel.Email                         Email message channel
* PushPlus.Channel.Email.Enable                  Enable Email message channel
* PushPlus.Channel.Email.SendLimit               Email message channel send limit
* PushPlus.Channel.Email.SendLimitInterval       Email message channel limit period (days)
* PushPlus.Channel.Sms                           SMS message channel
* PushPlus.Channel.Sms.Enable                    Enable SMS message channel
* PushPlus.Channel.Sms.SendLimit                 SMS message channel send limit
* PushPlus.Channel.Sms.SendLimitInterval         SMS message channel limit period (days)

## Configuration

```json
{
  "PushPlus": {
    "Security": {
      "Token": "your-pushplus-token",      // Token obtained from PushPlus platform
      "SecretKey": "your-pushplus-secret"  // Secret key obtained from PushPlus platform
    }
  }
}
```

## Basic Usage

1. Configure PushPlus Credentials
   * Get Token and Secret key from PushPlus platform
   * Set Token and Secret key in configuration file

2. Send Messages
   ```csharp
   public class YourService
   {
       private readonly IPushPlusMessageSender _messageSender;

       public YourService(IPushPlusMessageSender messageSender)
       {
           _messageSender = messageSender;
       }

       public async Task SendMessageAsync()
       {
           // Send WeChat message
           await _messageSender.SendWeChatAsync(
               title: "Message Title",
               content: "Message Content",
               topic: "Optional Topic",
               template: PushPlusMessageTemplate.Html
           );

           // Send WeWork message
           await _messageSender.SendWeWorkAsync(
               title: "Message Title",
               content: "Message Content"
           );

           // Send Email
           await _messageSender.SendEmailAsync(
               title: "Email Title",
               content: "Email Content"
           );

           // Send SMS
           await _messageSender.SendSmsAsync(
               title: "SMS Title",
               content: "SMS Content"
           );

           // Send Webhook message
           await _messageSender.SendWebhookAsync(
               title: "Message Title",
               content: "Message Content",
               webhook: "webhook-url"
           );
       }
   }
   ```

## Message Templates

* Html - HTML format (default)
* Text - Plain text format
* Json - JSON format
* Markdown - Markdown format

## More Information

* [PushPlus Documentation](https://www.pushplus.plus/doc/guide/openApi.html)
* [ABP Framework](https://abp.io/)
