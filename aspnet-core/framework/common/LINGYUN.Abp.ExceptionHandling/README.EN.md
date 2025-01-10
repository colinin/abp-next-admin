# LINGYUN.Abp.ExceptionHandling

A secondary extension based on the ABP framework's **IExceptionSubscriber** interface, used for customizing exception notification methods.

## Features

* Provides unified exception handling and notification mechanism
* Supports custom exception handlers
* Supports exception notification filtering
* Supports integration with other notification modules (such as email, real-time notifications, etc.)

## Configuration and Usage

Just configure **AbpExceptionHandlingOptions** to define which exceptions need to send notifications.

```csharp
    [DependsOn(
        typeof(AbpExceptionHandlingModule)
        )]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Customize exception handling
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                // Add exception types that need to be handled
                options.Handlers.Add<AbpException>();
            });
        }
    }
```

## Configuration Options

* `Handlers`: List of exception handlers, used to define which exception types need to be handled
* `HasNotifierError`: Check if an exception needs to send notifications

## Extension Modules

* [LINGYUN.Abp.ExceptionHandling.Emailing](../LINGYUN.Abp.ExceptionHandling.Emailing/README.EN.md): Exception email notification module
* [LINGYUN.Abp.ExceptionHandling.Notifications](../../../modules/realtime-notifications/LINGYUN.Abp.ExceptionHandling.Notifications/README.EN.md): Exception real-time notification module

## More

For more information and configuration examples, please refer to the [documentation](https://github.com/colinin/abp-next-admin).
