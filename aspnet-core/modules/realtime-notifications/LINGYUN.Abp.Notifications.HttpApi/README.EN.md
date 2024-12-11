# LINGYUN.Abp.Notifications.HttpApi

The HTTP API module of the notification system, providing REST API interfaces for the notification system.

## Features

* Notification management API
* Notification subscription API
* Notification status management API
* Support for API versioning
* Support for Swagger documentation

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Endpoints

### NotificationController

* GET /api/notifications/{id} - Get notification details
* GET /api/notifications - Get notification list
* DELETE /api/notifications/{id} - Delete notification
* PUT /api/notifications/{id}/read - Mark notification as read
* PUT /api/notifications/read - Mark all notifications as read

### NotificationSubscriptionController

* POST /api/notifications/subscriptions - Subscribe to notification
* DELETE /api/notifications/subscriptions - Unsubscribe from notification
* GET /api/notifications/subscribers - Get list of assignable subscribers
* GET /api/notifications/subscriptions - Get list of subscribed notifications

## Basic Usage

1. Configure Startup
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<YourHttpApiModule>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.InitializeApplication();
    }
}
```

2. API Call Examples
```bash
# Get notification list
curl -X GET "https://localhost:44300/api/notifications"

# Subscribe to notification
curl -X POST "https://localhost:44300/api/notifications/subscriptions" \
     -H "Content-Type: application/json" \
     -d '{"notificationName":"YourNotification"}'
```

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
