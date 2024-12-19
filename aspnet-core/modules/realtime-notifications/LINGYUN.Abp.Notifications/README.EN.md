# LINGYUN.Abp.Notifications

[简体中文](./README.md) | English

Real-time notification base module.

## Features

* Support multiple notification types (System, User, Application, Service Callback)
* Support multiple notification lifetimes (One-time, Persistent)
* Support multiple notification content types (Text, JSON, HTML, Markdown)
* Support multiple notification severity levels (Success, Info, Warning, Error, Fatal)
* Support notification subscription management
* Support notification status management (Read/Unread)
* Multi-tenancy support
* Localization support
* Custom notification provider support

## Module Dependencies

```csharp
[DependsOn(typeof(AbpNotificationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

### 1. Send Notification

```csharp
public class MyService
{
    private readonly INotificationSender _notificationSender;

    public MyService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendNotificationAsync()
    {
        var data = new NotificationData();
        data.TrySetData("title", "Test Notification");
        data.TrySetData("message", "This is a test notification");

        await _notificationSender.SendNofiterAsync(
            "TestNotification",
            data,
            severity: NotificationSeverity.Info);
    }
}
```

### 2. Manage Notification Subscriptions

```csharp
public class MyService
{
    private readonly INotificationSubscriptionManager _subscriptionManager;

    public MyService(INotificationSubscriptionManager subscriptionManager)
    {
        _subscriptionManager = subscriptionManager;
    }

    public async Task SubscribeAsync(Guid userId)
    {
        await _subscriptionManager.SubscribeAsync(
            userId,
            "TestNotification");
    }
}
```

## Configuration Options

```json
{
  "Notifications": {
    "PublishProviders": [
      "SignalR"  // Optional notification provider
    ]
  }
}
```

## Notification Types

* Application - Platform notifications
* System - System notifications
* User - User notifications
* ServiceCallback - Service callback notifications

## Notification Lifetimes

* OnlyOne - One-time notifications
* Persistent - Persistent notifications

## Notification Content Types

* Text - Text
* Json - JSON
* Html - HTML
* Markdown - Markdown

## Notification Severity Levels

* Success - Success
* Info - Information
* Warn - Warning
* Fatal - Fatal
* Error - Error

## Best Practices

1. Choose appropriate notification types and lifetimes based on actual requirements
2. Use notification severity levels appropriately, avoid overusing high-level notifications
3. Add proper localization support for notifications
4. Regularly clean up expired notification data

## Notes

1. Persistent notifications require implementation of INotificationStore interface
2. Custom notification providers require implementation of INotificationPublishProvider interface
3. Notification names should be unique and descriptive
4. Consider data isolation in multi-tenant scenarios
