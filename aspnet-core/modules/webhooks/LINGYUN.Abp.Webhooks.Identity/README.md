# LINGYUN.Abp.Webhooks.Identity

Webhook身份集成模块，提供与ABP身份系统的集成支持。

[English](README.EN.md)

## 功能特性

* 与ABP身份系统集成
* 支持用户和租户级别的Webhook
* 身份相关的Webhook事件

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksIdentityModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 处理身份相关的Webhook
```csharp
public class YourIdentityWebhookHandler : IWebhookHandler, ITransientDependency
{
    public async Task HandleWebhookAsync(WebhookPayload webhook)
    {
        if (webhook.WebhookName == "User.Created")
        {
            // 处理用户创建事件
        }
    }
}
```

2. 发布身份相关的Webhook
```csharp
public class YourService
{
    private readonly IWebhookPublisher _webhookPublisher;

    public YourService(IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async Task PublishIdentityWebhook()
    {
        await _webhookPublisher.PublishAsync(
            webhookName: "User.Created",
            data: new { /* user data */ }
        );
    }
}
```
