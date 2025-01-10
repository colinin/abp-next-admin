# LINGYUN.Abp.Identity.Notifications

身份认证通知模块，提供身份认证相关的通知功能。

## 功能特性

* 扩展AbpNotificationsModule模块
* 提供身份认证相关的通知定义
* 支持会话过期通知
* 提供身份会话撤销事件处理

## 模块引用

```csharp
[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpDddDomainSharedModule),
    typeof(AbpIdentityDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 通知定义

### 会话通知

* `AbpIdentity.Session.Expiration` - 会话过期通知
  * 当用户会话过期时发送此通知
  * 通知用户其会话已经过期，需要重新登录

## 事件处理

### IdentitySessionRevokeEventHandler

处理身份会话撤销事件，当会话被撤销时：
* 发送会话过期通知给相关用户
* 通知用户需要重新登录

## 基本用法

1. 订阅会话过期通知
```csharp
public class YourNotificationHandler : INotificationHandler<SessionExpirationNotification>
{
    public async Task HandleNotificationAsync(SessionExpirationNotification notification)
    {
        // 处理会话过期通知
    }
}
```

2. 发送会话过期通知
```csharp
public class YourService
{
    private readonly INotificationSender _notificationSender;

    public YourService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendSessionExpirationNotificationAsync(Guid userId)
    {
        await _notificationSender.SendAsync(
            IdentityNotificationNames.Session.ExpirationSession,
            new NotificationData(),
            userIds: new[] { userId });
    }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [ABP通知系统文档](https://docs.abp.io/en/abp/latest/Notification-System)
