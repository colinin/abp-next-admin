# LINGYUN.Abp.BackgroundTasks.ExceptionHandling

Background job execution exception notification implementation, using Email to send notifications by default.

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksExceptionHandlingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Action Parameters

** Specify the following parameters in the job definition to send notifications when the job execution fails:

* to        Required, recipient email address
* from      Optional, sender name in email header
* body      Optional, email content (required if template name is not specified)
* subject   Optional, email subject
* template  Optional, email template
* context   Optional, context parameters when using template
* culture   Optional, template culture when using template

## Features

- Supports email-based exception notifications for background jobs
- Customizable email templates with localization support
- Rich context information in notifications including:
  - Tenant name (if applicable)
  - Job group
  - Job name
  - Job ID
  - Job type
  - Trigger time
  - Error message
