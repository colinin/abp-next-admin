# LINGYUN.Abp.Webhooks.Saas

Webhook SaaS集成模块，提供与ABP SaaS系统的集成支持。

[English](README.EN.md)

## 功能特性

* 与ABP SaaS系统集成
* 支持多租户Webhook
* SaaS相关的Webhook事件

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksSaasModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 处理SaaS相关的Webhook
```csharp
public class YourSaasWebhookHandler : IWebhookHandler, ITransientDependency
{
    public async Task HandleWebhookAsync(WebhookPayload webhook)
    {
        if (webhook.WebhookName == "Tenant.Created")
        {
            // 处理租户创建事件
        }
    }
}
```

2. 发布SaaS相关的Webhook
```csharp
public class YourService
{
    private readonly IWebhookPublisher _webhookPublisher;

    public YourService(IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async Task PublishSaasWebhook()
    {
        await _webhookPublisher.PublishAsync(
            webhookName: "Tenant.Created",
            data: new { /* tenant data */ }
        );
    }
}
```
