# LINGYUN.Abp.Elsa.Activities.Notifications

Notification activities integration module for Elsa workflow

## Features

* Provides **SendNotification** activity for sending notifications
  * Support sending to multiple users
  * Support notification data and template content
  * Support JavaScript and Liquid syntax
  * Support setting notification severity
  * Integration with ABP framework's `INotificationSender` interface
  * Support multi-tenancy

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesNotificationsModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "Notification": true    // Enable notification activities
    }
}
```

## Activity Parameters

* **NotificationName**: Name of the registered notification
* **NotificationData**: Notification data or template content
* **To**: List of recipient user IDs
* **Severity**: Notification severity (default is Info)

## Output Parameters

* **NotificationId**: ID of the sent notification

[中文文档](./README.md)
