# LINGYUN.Abp.Notifications.Application

通知系统的应用层模块，提供了通知系统的应用服务实现。

## 功能特性

* 通知管理服务
* 通知订阅服务
* 通知发布服务
* 通知查询服务
* 通知状态管理服务

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务

### INotificationAppService

* GetAsync - 获取通知详情
* GetListAsync - 获取通知列表
* DeleteAsync - 删除通知
* MarkReadAsync - 标记通知为已读
* MarkAllReadAsync - 标记所有通知为已读

### INotificationSubscriptionAppService

* SubscribeAsync - 订阅通知
* UnSubscribeAsync - 取消订阅通知
* GetAssignableSubscribersAsync - 获取可分配的订阅者列表
* GetSubscribedListAsync - 获取已订阅的通知列表

## 基本用法

1. 发送通知
```csharp
await NotificationAppService.PublishAsync(
    name: "YourNotification",
    data: new NotificationData(),
    userIds: new[] { CurrentUser.Id });
```

2. 管理通知订阅
```csharp
await NotificationSubscriptionAppService.SubscribeAsync(
    notificationName: "YourNotification");
```

## 更多信息

* [ABP文档](https://docs.abp.io)
