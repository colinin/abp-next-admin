# LINGYUN.Abp.BackgroundTasks.Notifications

Background job execution notification events.

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksNotificationsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Action Parameters

** Notification content formatting parameters, which will send real-time notifications after job execution:

* push-provider   Optional, specify message push provider
* use-template   Optional, template name for formatting notification content
* content        Optional, notification content (required if template name is not specified)
* culture        Optional, template culture when using template

## Features

### Notification Types
- Job Success Notification
- Job Failure Notification
- Job Completion Notification

### Supported Push Providers
- SignalR (real-time notification)
- SMS (SMS notification)
- Emailing (email notification)
- WeChat.MiniProgram (WeChat Mini Program)
- WxPusher (WxPusher WeChat push service)
- PushPlus (PushPlus multi-platform push service)

### Notification Features
- Multi-tenant support
- Localization support
- Template-based content formatting
- Multiple push provider support
- Severity-based notifications (Info, Success, Warning, Error)
- Rich notification content with job details:
  - Job ID
  - Job Group
  - Job Name
  - Job Type
  - Trigger Time
  - Tenant Name (if applicable)
  - Error Message (for failed jobs)
