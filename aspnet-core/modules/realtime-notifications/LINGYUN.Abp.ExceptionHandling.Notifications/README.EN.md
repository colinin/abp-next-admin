# LINGYUN.Abp.ExceptionHandling.Notifications

A real-time notification type based on the ABP framework's **IExceptionSubscriber** interface, used to send exception information to users through real-time notifications.

## Features

* Supports real-time exception notifications
* Supports multi-tenancy
* Supports notification templates
* Supports system-level notifications
* Integrated with common notification module

## Configuration and Usage

Before use, you need to configure **AbpExceptionHandlingOptions** to define which exceptions need notifications.

```csharp
    [DependsOn(
        typeof(AbpNotificationsExceptionHandlingModule)
        )]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Customize exceptions to handle
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                // Add exception types that need to be handled
                options.Handlers.Add<AbpException>();
            });
        }
    }
```

## Notification Content

Exception notifications include the following information:
* `header`: Exception notification header information
* `footer`: Exception notification footer information
* `loglevel`: Log level
* `stackTrace`: Exception stack trace information

## Notification Names

The module uses the following notification names:
* `NotificationsCommonNotificationNames.ExceptionHandling`: Exception handling notification name

## Notification Template

* Sender: System
* Notification Level: Error
* Multi-tenant Support: Yes

## Dependencies

* `AbpExceptionHandlingModule`: Base exception handling module
* `AbpNotificationsCommonModule`: Common notification module

## Related Links

* [Base Exception Handling Module](../../../framework/common/LINGYUN.Abp.ExceptionHandling/README.EN.md)
* [Exception Email Notification Module](../../../framework/common/LINGYUN.Abp.ExceptionHandling.Emailing/README.EN.md)

## More

For more information and configuration examples, please refer to the [documentation](https://github.com/colinin/abp-next-admin).
