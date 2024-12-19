# LINGYUN.Abp.Notifications.Core

The core module of the notification system, providing basic functionality and definitions for the notification system.

## Features

* Notification definition management
* Notification group definition management
* Extensible notification provider mechanism
* Support for custom notification definition providers

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Notifications": {
    "DeletedNotifications": [], // List of notification definitions to be deleted
    "DeletedNotificationGroups": [] // List of notification group definitions to be deleted
  }
}
```

## Basic Usage

1. Implement custom notification definition provider
```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        // Define notifications
        context.Add(
            new NotificationDefinition(
                name: "YourNotification",
                displayName: L("YourNotification"),
                description: L("YourNotificationDescription"),
                notificationType: NotificationType.Application,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: true)
        );
    }
}
```

2. Register notification definition provider
```csharp
Configure<AbpNotificationsOptions>(options =>
{
    options.DefinitionProviders.Add<YourNotificationDefinitionProvider>();
});
```

## More Information

* [ABP Documentation](https://docs.abp.io)
