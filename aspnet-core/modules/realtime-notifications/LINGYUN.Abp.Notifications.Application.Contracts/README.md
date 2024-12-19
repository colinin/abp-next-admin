# LINGYUN.Abp.Notifications.Application.Contracts

通知系统的应用层契约模块，提供了通知系统的应用服务接口定义和数据传输对象。

## 功能特性

* 通知应用服务接口定义
* 通知订阅应用服务接口定义
* 通知数据传输对象（DTO）定义
* 通知权限定义

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务接口

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

## 数据传输对象

### NotificationInfo

* Id - 通知唯一标识
* NotificationName - 通知名称
* Data - 通知数据
* CreationTime - 创建时间
* Type - 通知类型
* Severity - 通知严重程度

### NotificationSubscriptionInfo

* NotificationName - 通知名称
* DisplayName - 显示名称
* Description - 描述
* IsSubscribed - 是否已订阅

## 权限定义

* Notifications - 通知管理
  * Notifications.Manage - 管理通知
  * Notifications.Delete - 删除通知
  * Notifications.Subscribe - 订阅通知

## 基本用法

1. 实现通知应用服务
```csharp
public class NotificationAppService : ApplicationService, INotificationAppService
{
    public async Task<NotificationInfo> GetAsync(Guid id)
    {
        // 实现获取通知详情的逻辑
    }
}
```

## 更多信息

* [ABP文档](https://docs.abp.io)
