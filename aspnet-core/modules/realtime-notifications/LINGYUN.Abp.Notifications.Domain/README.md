# LINGYUN.Abp.Notifications.Domain

通知系统的领域层模块，提供了通知系统的领域模型和业务逻辑。

## 功能特性

* 通知实体定义
* 通知订阅管理
* 通知状态管理
* 通知数据扩展支持
* 支持自定义通知数据

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 领域模型

### Notification

* Id - 通知唯一标识
* Name - 通知名称
* NotificationData - 通知数据
* CreationTime - 创建时间
* Type - 通知类型
* Severity - 通知严重程度
* ExtraProperties - 扩展属性

### NotificationSubscription

* UserId - 用户标识
* NotificationName - 通知名称
* CreationTime - 创建时间

## 基本用法

1. 创建通知
```csharp
var notification = new Notification(
    id: GuidGenerator.Create(),
    name: "YourNotification",
    data: new NotificationData(),
    tenantId: CurrentTenant.Id);
```

2. 管理通知订阅
```csharp
await NotificationSubscriptionManager.SubscribeAsync(
    userId: CurrentUser.Id,
    notificationName: "YourNotification");
```

## 更多信息

* [ABP文档](https://docs.abp.io)
