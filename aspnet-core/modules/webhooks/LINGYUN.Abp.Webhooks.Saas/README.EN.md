# LINGYUN.Abp.Webhooks.Saas

Webhook SaaS integration module that provides integration with ABP SaaS system.

[简体中文](README.md)

## Features

* Integration with ABP SaaS system
* Support for multi-tenant webhooks
* SaaS-related webhook events

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksSaasModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Handle SaaS-related Webhooks
```csharp
public class YourSaasWebhookHandler : IWebhookHandler, ITransientDependency
{
    public async Task HandleWebhookAsync(WebhookPayload webhook)
    {
        if (webhook.WebhookName == "Tenant.Created")
        {
            // Handle tenant creation event
        }
    }
}
```

2. Publish SaaS-related Webhooks
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
