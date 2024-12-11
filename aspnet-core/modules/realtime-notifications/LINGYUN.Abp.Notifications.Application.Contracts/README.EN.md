# LINGYUN.Abp.Notifications.Application.Contracts

The application layer contracts module of the notification system, providing application service interface definitions and data transfer objects.

## Features

* Notification application service interface definitions
* Notification subscription application service interface definitions
* Notification data transfer object (DTO) definitions
* Notification permission definitions

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Service Interfaces

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

## Data Transfer Objects

### NotificationInfo

* Id - Unique identifier for the notification
* NotificationName - Notification name
* Data - Notification data
* CreationTime - Creation time
* Type - Notification type
* Severity - Notification severity

### NotificationSubscriptionInfo

* NotificationName - Notification name
* DisplayName - Display name
* Description - Description
* IsSubscribed - Subscription status

## Permission Definitions

* Notifications - Notification management
  * Notifications.Manage - Manage notifications
  * Notifications.Delete - Delete notifications
  * Notifications.Subscribe - Subscribe to notifications

## Basic Usage

1. Implement notification application service
```csharp
public class NotificationAppService : ApplicationService, INotificationAppService
{
    public async Task<NotificationInfo> GetAsync(Guid id)
    {
        // Implement logic to get notification details
    }
}
```

## More Information

* [ABP Documentation](https://docs.abp.io)
