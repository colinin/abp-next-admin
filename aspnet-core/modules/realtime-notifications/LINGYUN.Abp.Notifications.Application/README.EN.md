# LINGYUN.Abp.Notifications.Application

The application layer module of the notification system, providing application service implementations for the notification system.

## Features

* Notification management service
* Notification subscription service
* Notification publishing service
* Notification query service
* Notification status management service

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Services

### INotificationAppService

* GetAsync - Get notification details
* GetListAsync - Get notification list
* DeleteAsync - Delete notification
* MarkReadAsync - Mark notification as read
* MarkAllReadAsync - Mark all notifications as read

### INotificationSubscriptionAppService

* SubscribeAsync - Subscribe to notification
* UnSubscribeAsync - Unsubscribe from notification
* GetAssignableSubscribersAsync - Get list of assignable subscribers
* GetSubscribedListAsync - Get list of subscribed notifications

## Basic Usage

1. Send notification
```csharp
await NotificationAppService.PublishAsync(
    name: "YourNotification",
    data: new NotificationData(),
    userIds: new[] { CurrentUser.Id });
```

2. Manage notification subscription
```csharp
await NotificationSubscriptionAppService.SubscribeAsync(
    notificationName: "YourNotification");
```

## More Information

* [ABP Documentation](https://docs.abp.io)
