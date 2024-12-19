# LINGYUN.Abp.Notifications.Domain

The domain layer module of the notification system, providing domain models and business logic for the notification system.

## Features

* Notification entity definition
* Notification subscription management
* Notification status management
* Notification data extension support
* Support for custom notification data

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Domain Models

### Notification

* Id - Unique identifier for the notification
* Name - Notification name
* NotificationData - Notification data
* CreationTime - Creation time
* Type - Notification type
* Severity - Notification severity
* ExtraProperties - Extension properties

### NotificationSubscription

* UserId - User identifier
* NotificationName - Notification name
* CreationTime - Creation time

## Basic Usage

1. Create notification
```csharp
var notification = new Notification(
    id: GuidGenerator.Create(),
    name: "YourNotification",
    data: new NotificationData(),
    tenantId: CurrentTenant.Id);
```

2. Manage notification subscriptions
```csharp
await NotificationSubscriptionManager.SubscribeAsync(
    userId: CurrentUser.Id,
    notificationName: "YourNotification");
```

## More Information

* [ABP Documentation](https://docs.abp.io)
