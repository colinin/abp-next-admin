# LINGYUN.Abp.Notifications.Common

The common module of the notification system, providing basic definitions and shared functionality.

## Features

* Notification Definitions
  * Notification group definitions
  * Notification type definitions
  * Notification level definitions
* Common Utilities
  * Notification data handling
  * Notification status management
* Extension Features
  * Notification data extensions
  * Notification provider extensions

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsCommonModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Definitions

### Notification Group

```csharp
public class NotificationGroupDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public bool AllowSubscriptionToClients { get; }
}
```

### Notification Definition

```csharp
public class NotificationDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public NotificationType NotificationType { get; }
    public NotificationLifetime Lifetime { get; }
    public bool AllowSubscriptionToClients { get; }
}
```

## Basic Usage

1. Define Notification Group
```csharp
public class YourNotificationGroupDefinitionProvider : NotificationGroupDefinitionProvider
{
    public override void Define(INotificationGroupDefinitionContext context)
    {
        context.Add(
            new NotificationGroupDefinition(
                name: "App.Notifications",
                displayName: L("AppNotifications"),
                description: L("AppNotificationsDescription")
            )
        );
    }
}
```

2. Define Notification
```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        context.Add(
            new NotificationDefinition(
                name: "App.NewMessage",
                displayName: L("NewMessage"),
                description: L("NewMessageDescription"),
                notificationType: NotificationType.Application,
                lifetime: NotificationLifetime.Persistent
            )
        );
    }
}
```

3. Use Notification Data Extensions
```csharp
public static class NotificationDataExtensions
{
    public static void SetTitle(this NotificationData data, string title)
    {
        data.ExtraProperties["Title"] = title;
    }

    public static string GetTitle(this NotificationData data)
    {
        return data.ExtraProperties.GetOrDefault("Title") as string;
    }
}
```

## More Information

* [ABP Documentation](https://docs.abp.io)
* [Notifications Documentation](https://docs.abp.io/en/abp/latest/Notifications)
