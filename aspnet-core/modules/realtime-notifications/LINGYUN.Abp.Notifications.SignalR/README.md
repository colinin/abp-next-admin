# LINGYUN.Abp.Notifications.SignalR

通知系统的SignalR模块，提供了基于SignalR的实时通知功能。

## 功能特性

* 实时通知推送
* 通知Hub实现
* 客户端连接管理
* 支持分组通知
* 支持用户在线状态管理

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsSignalRModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "SignalR": {
    "HubUrl": "/signalr-hubs/notifications",
    "UseMessagePack": false
  }
}
```

## Hub定义

### NotificationHub

* SubscribeToNotifications - 订阅通知
* UnsubscribeFromNotifications - 取消订阅通知
* GetNotifications - 获取通知列表
* MarkNotificationAsRead - 标记通知为已读
* MarkAllNotificationsAsRead - 标记所有通知为已读

## 基本用法

1. 服务端配置
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR()
                .AddMessagePackProtocol(); // 可选，使用MessagePack协议
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

2. 客户端使用
```javascript
// 连接到通知Hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalr-hubs/notifications")
    .build();

// 监听通知
connection.on("ReceiveNotification", (notification) => {
    console.log("收到新通知:", notification);
});

// 启动连接
await connection.start();

// 订阅通知
await connection.invoke("SubscribeToNotifications");
```

## 更多信息

* [ABP文档](https://docs.abp.io)
* [SignalR文档](https://docs.microsoft.com/aspnet/core/signalr)
