# LINGYUN.Abp.Notifications

[English](./README.EN.md) | 简体中文

实时通知基础模块。

## 功能

* 支持多种通知类型（系统、用户、应用、服务回调）
* 支持多种通知生命周期（一次性、持久化）
* 支持多种通知内容类型（文本、JSON、HTML、Markdown）
* 支持多种通知严重级别（成功、信息、警告、错误、致命）
* 支持通知订阅管理
* 支持通知状态管理（已读/未读）
* 支持多租户
* 支持本地化
* 支持自定义通知提供程序

## 模块依赖

```csharp
[DependsOn(typeof(AbpNotificationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

### 1. 发送通知

```csharp
public class MyService
{
    private readonly INotificationSender _notificationSender;

    public MyService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendNotificationAsync()
    {
        var data = new NotificationData();
        data.TrySetData("title", "测试通知");
        data.TrySetData("message", "这是一条测试通知");

        await _notificationSender.SendNofiterAsync(
            "TestNotification",
            data,
            severity: NotificationSeverity.Info);
    }
}
```

### 2. 管理通知订阅

```csharp
public class MyService
{
    private readonly INotificationSubscriptionManager _subscriptionManager;

    public MyService(INotificationSubscriptionManager subscriptionManager)
    {
        _subscriptionManager = subscriptionManager;
    }

    public async Task SubscribeAsync(Guid userId)
    {
        await _subscriptionManager.SubscribeAsync(
            userId,
            "TestNotification");
    }
}
```

## 配置选项

```json
{
  "Notifications": {
    "PublishProviders": [
      "SignalR"  // 可选的通知提供程序
    ]
  }
}
```

## 通知类型

* Application - 平台通知
* System - 系统通知
* User - 用户通知
* ServiceCallback - 服务回调通知

## 通知生命周期

* OnlyOne - 一次性通知
* Persistent - 持久化通知

## 通知内容类型

* Text - 文本
* Json - JSON
* Html - HTML
* Markdown - Markdown

## 通知严重级别

* Success - 成功
* Info - 信息
* Warn - 警告
* Fatal - 致命
* Error - 错误

## 最佳实践

1. 根据实际需求选择合适的通知类型和生命周期
2. 合理使用通知严重级别，避免滥用高级别通知
3. 为通知添加适当的本地化支持
4. 定期清理过期的通知数据

## 注意事项

1. 持久化通知需要实现 INotificationStore 接口
2. 自定义通知提供程序需要实现 INotificationPublishProvider 接口
3. 通知名称应该具有唯一性和描述性
4. 考虑多租户场景下的数据隔离
