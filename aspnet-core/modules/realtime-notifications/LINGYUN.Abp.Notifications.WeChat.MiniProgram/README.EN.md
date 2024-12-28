# LINGYUN.Abp.Notifications.WeChat.MiniProgram

WeChat Mini Program notification publishing module, providing functionality to send subscription messages to users through WeChat Mini Program.

## Features

* WeChat Mini Program subscription message sending
* Support for custom message templates
* Support for multi-language messages
* Support for different environments (development, trial, release)
* Integration with ABP notification system

## Module Reference

```csharp
[DependsOn(typeof(AbpNotificationsWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

> Note: This configuration will be removed in the next major WeChat-related version and merged into LINGYUN.Abp.WeChat.MiniProgram.AbpWeChatMiniProgramOptions

```json
{
  "Notifications": {
    "WeChat": {
      "MiniProgram": {
        "DefaultMsgPrefix": "",           // Default message prefix
        "DefaultTemplateId": "",          // Default Mini Program template ID
        "DefaultState": "developer",      // Default Mini Program type: developer/trial/formal
        "DefaultLanguage": "zh_CN"        // Default Mini Program language
      }
    }
  }
}
```

## Usage

### Sending Notifications

```csharp
public class YourService
{
    private readonly INotificationPublisher _notificationPublisher;

    public YourService(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task SendNotificationAsync()
    {
        await _notificationPublisher.PublishAsync(
            "WeChatMiniProgram.Notification", // Notification name
            new NotificationData(),           // Notification data
            userIds: new[] { "userId" },      // Recipient user IDs
            tenantIds: new[] { "tenantId" }   // Tenant IDs
        );
    }
}
```

### Custom Notification Handling

```csharp
public class YourNotificationHandler : 
    INotificationHandler<WeChatMiniProgramNotification>
{
    public async Task HandleNotificationAsync(
        WeChatMiniProgramNotification notification)
    {
        // Handle notification
    }
}
```

## More Documentation

* [Chinese Documentation](README.md)
* [WeChat Mini Program Subscribe Message Documentation](https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/subscribe-message.html)
