# LINGYUN.Abp.Webhooks.Identity

Webhook identity integration module that provides integration with ABP identity system.

[简体中文](README.md)

## Features

* Integration with ABP identity system
* Support for user and tenant level webhooks
* Identity-related webhook events

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksIdentityModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Handle Identity-related Webhooks
```csharp
public class YourIdentityWebhookHandler : IWebhookHandler, ITransientDependency
{
    public async Task HandleWebhookAsync(WebhookPayload webhook)
    {
        if (webhook.WebhookName == "User.Created")
        {
            // Handle user creation event
        }
    }
}
```

2. Publish Identity-related Webhooks
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
