# LINGYUN.Abp.Identity.Notifications

Identity authentication notification module, providing notification functionality related to identity authentication.

## Features

* Extends AbpNotificationsModule module
* Provides notification definitions related to identity authentication
* Supports session expiration notifications
* Provides identity session revocation event handling

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpDddDomainSharedModule),
    typeof(AbpIdentityDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Notification Definitions

### Session Notifications

* `AbpIdentity.Session.Expiration` - Session expiration notification
  * Sent when a user's session expires
  * Notifies the user that their session has expired and they need to log in again

## Event Handling

### IdentitySessionRevokeEventHandler

Handles identity session revocation events. When a session is revoked:
* Sends session expiration notification to relevant users
* Notifies users they need to log in again

## Basic Usage

1. Subscribe to session expiration notifications
```csharp
public class YourNotificationHandler : INotificationHandler<SessionExpirationNotification>
{
    public async Task HandleNotificationAsync(SessionExpirationNotification notification)
    {
        // Handle session expiration notification
    }
}
```

2. Send session expiration notification
```csharp
public class YourService
{
    private readonly INotificationSender _notificationSender;

    public YourService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendSessionExpirationNotificationAsync(Guid userId)
    {
        await _notificationSender.SendAsync(
            IdentityNotificationNames.Session.ExpirationSession,
            new NotificationData(),
            userIds: new[] { userId });
    }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [ABP Notification System Documentation](https://docs.abp.io/en/abp/latest/Notification-System)
