# LINGYUN.Abp.Notifications.Domain.Shared

The shared domain layer module of the notification system, providing shared constants, enums, and other domain objects for the notification system.

## Features

* Notification type definition
* Notification severity definition
* Notification status definition
* Notification lifetime definition
* Notification constant definition

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Enum Definitions

### NotificationType

* Application - Application notification
* System - System notification
* User - User notification

### NotificationSeverity

* Info - Information
* Success - Success
* Warn - Warning
* Error - Error
* Fatal - Fatal error

### NotificationLifetime

* Persistent - Persistent notification
* OnlyOne - One-time notification

## Basic Usage

1. Use notification type
```csharp
var notificationType = NotificationType.Application;
```

2. Use notification severity
```csharp
var severity = NotificationSeverity.Info;
```

## More Information

* [ABP Documentation](https://docs.abp.io)
