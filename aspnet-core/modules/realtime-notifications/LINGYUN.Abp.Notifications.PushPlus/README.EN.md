# LINGYUN.Abp.Notifications.PushPlus

PushPlus implementation of the notification module

Enables applications to publish real-time notifications through PushPlus

## Features

* Support for multiple message types
  * Text messages
  * HTML messages
  * Markdown messages
  * Image messages
  * Custom template messages
* Message callback support
  * Custom callback URL support
  * Message sending status callback support
* Multi-channel push support
  * WeChat Official Account
  * WeChat Work
  * Email
  * SMS
  * WebHook
* Group message support
  * Group push support
  * Group management support

## Module Reference

```csharp
[DependsOn(typeof(AbpNotificationsPushPlusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "PushPlus": {
    "Token": "Your PushPlus Token",
    "DefaultChannel": "wechat",          // Default push channel: wechat/webhook/mail/sms
    "DefaultTemplate": "html",           // Default message template: html/json/markdown/txt
    "DefaultWebhook": "",               // Default Webhook URL
    "DefaultCallbackUrl": ""            // Default callback URL
  }
}
```

## Basic Usage

1. Configure PushPlus Service
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpNotificationsPushPlusOptions>(options =>
    {
        options.Token = "Your PushPlus Token";
        options.DefaultChannel = "wechat";
        options.DefaultTemplate = "html";
    });
}
```

2. Send Notification
```csharp
public class YourService
{
    private readonly INotificationSender _notificationSender;

    public YourService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendNotificationAsync()
    {
        var notificationData = new NotificationData();
        notificationData.TrySetData("title", "Message Title");
        notificationData.TrySetData("content", "Message Content");
        notificationData.SetWebhook("https://your-webhook.com");
        notificationData.SetCallbackUrl("https://your-callback.com");

        await _notificationSender.SendNofiterAsync(
            "YourNotification",
            notificationData,
            userIds: new[] { CurrentUser.Id }
        );
    }
}
```

## Important Notes

1. Registration on the PushPlus platform and obtaining a Token is required before use.
2. Ensure the configured Token is valid and has sufficient permissions.
3. Callback URL must be accessible from the public internet.
4. Different channels may have different message format requirements.
5. Message sending depends on PushPlus API, ensure network connectivity is stable.

## More Information

* [PushPlus Documentation](http://www.pushplus.plus/doc/)
* [ABP Documentation](https://docs.abp.io)
