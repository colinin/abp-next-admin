# LINGYUN.Abp.Notifications.SignalR

The SignalR module of the notification system, providing real-time notification functionality based on SignalR.

## Features

* Real-time notification push
* Notification Hub implementation
* Client connection management
* Support for group notifications
* Support for user online status management

## Module References

```csharp
[DependsOn(typeof(AbpNotificationsSignalRModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "SignalR": {
    "HubUrl": "/signalr-hubs/notifications",
    "UseMessagePack": false
  }
}
```

## Hub Definitions

### NotificationHub

* SubscribeToNotifications - Subscribe to notifications
* UnsubscribeFromNotifications - Unsubscribe from notifications
* GetNotifications - Get notification list
* MarkNotificationAsRead - Mark notification as read
* MarkAllNotificationsAsRead - Mark all notifications as read

## Basic Usage

1. Server Configuration
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR()
                .AddMessagePackProtocol(); // Optional, use MessagePack protocol
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<NotificationHub>("/signalr-hubs/notifications");
        });
    }
}
```

2. Client Usage
```javascript
// Connect to notification hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalr-hubs/notifications")
    .build();

// Listen for notifications
connection.on("ReceiveNotification", (notification) => {
    console.log("Received new notification:", notification);
});

// Start connection
await connection.start();

// Subscribe to notifications
await connection.invoke("SubscribeToNotifications");
```

## More Information

* [ABP Documentation](https://docs.abp.io)
* [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr)
