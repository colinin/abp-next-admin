# LINGYUN.Abp.Notifications.WxPusher

ABP notification module implemented through WxPusher, providing real-time notification functionality via WxPusher.

[简体中文](./README.md)

## Features

* Support for multiple message types
  * Text messages
  * HTML messages
  * Markdown messages
* Flexible message targeting
  * Send to specific users
  * Send to specific Topics
* Multi-language support
  * Localized message content
  * Multi-language titles and descriptions
* Feature toggle control
  * Message sending controlled by feature switches

## Installation

```bash
dotnet add package LINGYUN.Abp.Notifications.WxPusher
```

## Module Reference

```csharp
[DependsOn(typeof(AbpNotificationsWxPusherModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Configuration

### 1. Notification Definition Configuration

```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var notification = context.Create(
            name: "App.Notification.Test",
            displayName: L("TestNotification"))
            .WithContentType(MessageContentType.Text)  // Set message type
            .WithTopics(new List<int> { 1, 2 })       // Set message Topics
            .WithUrl("https://example.com");          // Set URL to jump to when clicking the message
    }
}
```

### 2. Sending Notifications

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
        var notificationData = new NotificationData();
        
        // Set message content
        notificationData.TrySetData("title", "Message Title");
        notificationData.TrySetData("message", "Message Content");
        notificationData.SetUrl("https://example.com");  // Set URL to jump to when clicking the message

        await _notificationPublisher.PublishAsync(
            "App.Notification.Test",              // Notification name
            notificationData,                     // Notification data
            userIds: new[] { "userId" },          // Recipient user IDs
            tenantIds: new[] { "tenantId" }       // Tenant IDs
        );
    }
}
```

## Important Notes

1. Implementation of `IWxPusherUserStore` interface is required to manage user associations with WxPusher.
2. Message sending depends on WxPusher API, ensure network connectivity is stable.
3. Set message content length reasonably to avoid exceeding WxPusher limits.
4. When using multi-language features, ensure localization resources are properly configured.

## Source Code

[LINGYUN.Abp.Notifications.WxPusher](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/modules/realtime-notifications/LINGYUN.Abp.Notifications.WxPusher)
