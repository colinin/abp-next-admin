# LINGYUN.Abp.Notifications.HttpApi

通知系统的HTTP API模块，提供了通知系统的REST API接口。

## 功能特性

* 通知管理API
* 通知订阅API
* 通知状态管理API
* 支持API版本控制
* 支持Swagger文档

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API接口

### NotificationController

* GET /api/notifications/{id} - 获取通知详情
* GET /api/notifications - 获取通知列表
* DELETE /api/notifications/{id} - 删除通知
* PUT /api/notifications/{id}/read - 标记通知为已读
* PUT /api/notifications/read - 标记所有通知为已读

### NotificationSubscriptionController

* POST /api/notifications/subscriptions - 订阅通知
* DELETE /api/notifications/subscriptions - 取消订阅通知
* GET /api/notifications/subscribers - 获取可分配的订阅者列表
* GET /api/notifications/subscriptions - 获取已订阅的通知列表

## 基本用法

1. 配置Startup
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

2. 调用API示例
```bash
# 获取通知列表
curl -X GET "https://localhost:44300/api/notifications"

# 订阅通知
curl -X POST "https://localhost:44300/api/notifications/subscriptions" \
     -H "Content-Type: application/json" \
     -d '{"notificationName":"YourNotification"}'
```

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ASP.NET Core文档](https://docs.microsoft.com/aspnet/core)
