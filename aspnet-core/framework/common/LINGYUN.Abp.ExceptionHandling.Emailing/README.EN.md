# LINGYUN.Abp.ExceptionHandling.Emailing

An email notification type based on the ABP framework's **IExceptionSubscriber** interface.

## Features

* Supports exception email notifications
* Supports custom email templates
* Supports multilingual email content
* Supports stack trace sending
* Supports mapping between exception types and recipient mailboxes

## Configuration and Usage

Before use, you need to configure **AbpExceptionHandlingOptions** to define which exceptions need notifications,
then configure **AbpEmailExceptionHandlingOptions** to define specific exception type notification methods.

```csharp
    [DependsOn(
        typeof(AbpEmailingExceptionHandlingModule)
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
            // Customize exception types that need email notifications
            Configure<AbpEmailExceptionHandlingOptions>(options =>
            {
                // Whether to send stack trace information
                options.SendStackTrace = true;
                // Default receiving email for unspecified exception receivers
                options.DefaultReceiveEmail = "colin.in@foxmail.com";
                // Specify which email to send for a certain exception type
                options.HandReceivedException<AbpException>("colin.in@foxmail.com");
            });
        }
    }
```

## Configuration Options

* `SendStackTrace`: Whether to include exception stack trace in the email
* `DefaultTitle`: Default email title
* `DefaultContentHeader`: Default email content header
* `DefaultContentFooter`: Default email content footer
* `DefaultReceiveEmail`: Default exception receiving email
* `Handlers`: Dictionary mapping exception types to receiving emails

## Localization Resources

The module includes localization resources in the following languages:
* en
* zh-Hans

## Related Links

* [Base Exception Handling Module](../LINGYUN.Abp.ExceptionHandling/README.EN.md)
* [Exception Real-time Notification Module](../../../modules/realtime-notifications/LINGYUN.Abp.ExceptionHandling.Notifications/README.EN.md)

## More

For more information and configuration examples, please refer to the [documentation](https://github.com/colinin/abp-next-admin).
