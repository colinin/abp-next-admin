# LINGYUN.Abp.Notifications.Core

通知系统的核心模块，提供了通知系统的基础功能和定义。

## 功能特性

* 通知定义管理
* 通知组定义管理
* 可扩展的通知提供者机制
* 支持自定义通知定义提供者

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Notifications": {
    "DeletedNotifications": [], // 需要删除的通知定义列表
    "DeletedNotificationGroups": [] // 需要删除的通知组定义列表
  }
}
```

## 基本用法

1. 实现自定义通知定义提供者
```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        // 定义通知
        context.Add(
            new NotificationDefinition(
                name: "YourNotification",
                displayName: L("YourNotification"),
                description: L("YourNotificationDescription"),
                notificationType: NotificationType.Application,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: true)
        );
    }
}
```

2. 注册通知定义提供者
```csharp
Configure<AbpNotificationsOptions>(options =>
{
    options.DefinitionProviders.Add<YourNotificationDefinitionProvider>();
});
```

## 更多信息

* [ABP文档](https://docs.abp.io)
