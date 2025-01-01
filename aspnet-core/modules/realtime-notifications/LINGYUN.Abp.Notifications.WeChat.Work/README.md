# LINGYUN.Abp.Notifications.WeChat.Work

企业微信通知发布模块，提供通过企业微信应用向用户发送消息的功能。

## 功能特性

* 支持多种消息类型
  * 文本消息
  * 文本卡片消息
  * Markdown消息
* 灵活的消息发送目标
  * 支持发送到指定用户
  * 支持发送到指定部门
  * 支持发送到指定标签
  * 支持发送到多个应用
* 多语言支持
  * 支持本地化消息内容
  * 支持多语言标题和描述
* 特性开关控制
  * 支持通过功能开关控制消息发送
* 扩展性
  * 支持自定义通知定义
  * 支持自定义消息处理

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

### 1. 基础配置

消息发送需要在企业微信后台配置应用，并获取相应的应用ID（AgentId）。

### 2. 通知定义配置

```csharp
public class YourNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var notification = context.Create(
            name: "App.Notification.Test",
            displayName: L("TestNotification"))
            .WithAgentId("1000001")               // 设置应用ID
            .WithParty("1|2|3")                   // 设置接收部门
            .WithTag("TagId1|TagId2")             // 设置接收标签
            .WithAllAgent();                      // 发送到所有应用
    }
}
```

### 3. 发送通知

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
        notificationData.TrySetData("description", "消息描述");

        // 设置发送目标
        notificationData.SetAgentId("1000001");   // 设置应用ID
        notificationData.SetParty("1|2|3");       // 设置接收部门
        notificationData.SetTag("TagId1|TagId2"); // 设置接收标签
        notificationData.WithAllAgent();          // 发送到所有应用

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

1. 发送消息时需要确保企业微信应用配置正确，包括应用ID和相关权限。
2. 部门ID和标签ID需要与企业微信后台配置的一致。
3. 多个接收者（部门/标签）使用'|'分隔，最多支持100个。
4. 使用 `WithAllAgent()` 时会向所有配置的应用发送消息。
5. 消息发送依赖于企业微信API，需要确保网络连接正常。

## 更多文档

* [English Documentation](README.EN.md)
* [企业微信应用消息开发文档](https://developer.work.weixin.qq.com/document/path/90236)
