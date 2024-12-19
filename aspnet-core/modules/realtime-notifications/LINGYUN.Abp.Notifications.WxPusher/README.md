# LINGYUN.Abp.Notifications.WxPusher

通过WxPusher实现的ABP通知模块，提供通过WxPusher发送实时通知的功能。

[English](./README.EN.md)

## 功能特性

* 支持多种消息类型
  * 文本消息
  * HTML消息
  * Markdown消息
* 灵活的消息发送目标
  * 支持发送到指定用户
  * 支持发送到指定Topic
* 多语言支持
  * 支持本地化消息内容
  * 支持多语言标题和描述
* 特性开关控制
  * 支持通过功能开关控制消息发送

## 安装

```bash
dotnet add package LINGYUN.Abp.Notifications.WxPusher
```

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsWxPusherModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 配置说明

### 1. 通知定义配置

```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var notification = context.Create(
            name: "App.Notification.Test",
            displayName: L("TestNotification"))
            .WithContentType(MessageContentType.Text)  // 设置消息类型
            .WithTopics(new List<int> { 1, 2 })       // 设置消息Topic
            .WithUrl("https://example.com");          // 设置点击消息跳转的URL
    }
}
```

### 2. 发送通知

```csharp
public class YourService
{
    private readonly INotificationPublisher _notificationPublisher;

    public YourService(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task SendNotificationAsync()
    {
        var notificationData = new NotificationData();
        
        // 设置消息内容
        notificationData.TrySetData("title", "消息标题");
        notificationData.TrySetData("message", "消息内容");
        notificationData.SetUrl("https://example.com");  // 设置点击消息跳转的URL

        await _notificationPublisher.PublishAsync(
            "App.Notification.Test",              // 通知名称
            notificationData,                     // 通知数据
            userIds: new[] { "userId" },          // 接收用户ID列表
            tenantIds: new[] { "tenantId" }       // 租户ID列表
        );
    }
}
```

## 注意事项

1. 需要实现 `IWxPusherUserStore` 接口来管理用户与WxPusher的关联关系。
2. 消息发送依赖于WxPusher API，需要确保网络连接正常。
3. 请合理设置消息内容长度，避免超过WxPusher的限制。
4. 使用多语言功能时，需要确保已正确配置本地化资源。

## 源码位置

[LINGYUN.Abp.Notifications.WxPusher](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/modules/realtime-notifications/LINGYUN.Abp.Notifications.WxPusher)
