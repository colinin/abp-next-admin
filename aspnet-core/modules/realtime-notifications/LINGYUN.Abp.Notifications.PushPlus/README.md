# LINGYUN.Abp.Notifications.PushPlus

通知模块的PushPlus实现  

使应用可通过PushPlus发布实时通知  

## 功能特性

* 支持多种消息类型
  * 文本消息
  * HTML消息
  * Markdown消息
  * 图片消息
  * 自定义模板消息
* 支持消息回调
  * 支持自定义回调URL
  * 支持消息发送状态回调
* 支持多渠道推送
  * 微信公众号
  * 企业微信
  * 邮件
  * 短信
  * WebHook
* 支持群组消息
  * 支持群组推送
  * 支持群组管理

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsPushPlusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "PushPlus": {
    "Token": "你的PushPlus Token",
    "DefaultChannel": "wechat",          // 默认推送渠道：wechat/webhook/mail/sms
    "DefaultTemplate": "html",           // 默认消息模板：html/json/markdown/txt
    "DefaultWebhook": "",               // 默认Webhook地址
    "DefaultCallbackUrl": ""            // 默认回调地址
  }
}
```

## 基本用法

1. 配置PushPlus服务
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpNotificationsPushPlusOptions>(options =>
    {
        options.Token = "你的PushPlus Token";
        options.DefaultChannel = "wechat";
        options.DefaultTemplate = "html";
    });
}
```

2. 发送通知
```csharp
public class YourService
{
    private readonly INotificationSender _notificationSender;

    public YourService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task SendNotificationAsync()
    {
        var notificationData = new NotificationData();
        notificationData.TrySetData("title", "消息标题");
        notificationData.TrySetData("content", "消息内容");
        notificationData.SetWebhook("https://your-webhook.com");
        notificationData.SetCallbackUrl("https://your-callback.com");

        await _notificationSender.SendNofiterAsync(
            "YourNotification",
            notificationData,
            userIds: new[] { CurrentUser.Id }
        );
    }
}
```

## 注意事项

1. 使用前需要在PushPlus平台注册并获取Token。
2. 确保配置的Token有效且具有足够的权限。
3. 回调URL必须是可以公网访问的地址。
4. 不同渠道可能有不同的消息格式要求。
5. 消息发送依赖于PushPlus API，需要确保网络连接正常。

## 更多信息

* [PushPlus官方文档](http://www.pushplus.plus/doc/)
* [ABP文档](https://docs.abp.io)
