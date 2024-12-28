# LINGYUN.Abp.Notifications.WeChat.MiniProgram

微信小程序通知发布模块，提供通过微信小程序向用户发送订阅消息的功能。

## 功能特性

* 微信小程序订阅消息发送
* 支持自定义消息模板
* 支持多语言消息
* 支持不同环境（开发版、体验版、正式版）
* 集成ABP通知系统

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

> 注意：此配置项将在下一个微信相关大版本移除，合并到 LINGYUN.Abp.WeChat.MiniProgram.AbpWeChatMiniProgramOptions

```json
{
  "Notifications": {
    "WeChat": {
      "MiniProgram": {
        "DefaultMsgPrefix": "",           // 默认消息头部标记
        "DefaultTemplateId": "",          // 默认小程序模板ID
        "DefaultState": "developer",      // 默认跳转小程序类型：developer（开发版）、trial（体验版）、formal（正式版）
        "DefaultLanguage": "zh_CN"        // 默认小程序语言
      }
    }
  }
}
```

## 使用方式

### 发送通知

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
        await _notificationPublisher.PublishAsync(
            "WeChatMiniProgram.Notification", // 通知名称
            new NotificationData(),           // 通知数据
            userIds: new[] { "userId" },      // 接收用户ID列表
            tenantIds: new[] { "tenantId" }   // 租户ID列表
        );
    }
}
```

### 自定义通知处理

```csharp
public class YourNotificationHandler : 
    INotificationHandler<WeChatMiniProgramNotification>
{
    public async Task HandleNotificationAsync(
        WeChatMiniProgramNotification notification)
    {
        // 处理通知
    }
}
```

## 更多文档

* [微信小程序通知发布模块文档](README.EN.md)
* [微信小程序订阅消息开发文档](https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/subscribe-message.html)
