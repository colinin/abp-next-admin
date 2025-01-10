# LINGYUN.Abp.Notifications.Common

通知系统的公共模块，提供了通知系统的基础定义和共享功能。

## 功能特性

* 通知定义
  * 通知组定义
  * 通知类型定义
  * 通知级别定义
* 通用工具类
  * 通知数据处理
  * 通知状态管理
* 扩展功能
  * 通知数据扩展
  * 通知提供者扩展

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsCommonModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基础定义

### 通知组

```csharp
public class NotificationGroupDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public bool AllowSubscriptionToClients { get; }
}
```

### 通知定义

```csharp
public class NotificationDefinition
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public NotificationType NotificationType { get; }
    public NotificationLifetime Lifetime { get; }
    public bool AllowSubscriptionToClients { get; }
}
```

## 基本用法

1. 定义通知组
```csharp
public class YourNotificationGroupDefinitionProvider : NotificationGroupDefinitionProvider
{
    public override void Define(INotificationGroupDefinitionContext context)
    {
        context.Add(
            new NotificationGroupDefinition(
                name: "App.Notifications",
                displayName: L("AppNotifications"),
                description: L("AppNotificationsDescription")
            )
        );
    }
}
```

2. 定义通知
```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        context.Add(
            new NotificationDefinition(
                name: "App.NewMessage",
                displayName: L("NewMessage"),
                description: L("NewMessageDescription"),
                notificationType: NotificationType.Application,
                lifetime: NotificationLifetime.Persistent
            )
        );
    }
}
```

3. 使用通知数据扩展
```csharp
public static class NotificationDataExtensions
{
    public static void SetTitle(this NotificationData data, string title)
    {
        data.ExtraProperties["Title"] = title;
    }

    public static string GetTitle(this NotificationData data)
    {
        return data.ExtraProperties.GetOrDefault("Title") as string;
    }
}
```

## 更多信息

* [ABP文档](https://docs.abp.io)
* [通知系统文档](https://docs.abp.io/en/abp/latest/Notifications)
