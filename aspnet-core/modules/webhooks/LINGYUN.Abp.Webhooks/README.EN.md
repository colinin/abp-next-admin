# LINGYUN.Abp.Webhooks

Base webhook module that provides basic webhook definitions and functionality.

[简体中文](README.md)

## Features

* Basic webhook definitions
* Webhook publishing and subscription
* Webhook event data handling
* Webhook configuration management

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Define a Webhook
```csharp
public class YourWebhookDefinitionProvider : WebhookDefinitionProvider
{
    public override void Define(IWebhookDefinitionContext context)
    {
        context.Add(
            new WebhookDefinition(
                name: "YourWebhook",
                displayName: L("DisplayName:YourWebhook"),
                description: L("Description:YourWebhook")
            )
        );
    }
}
```

2. Publish a Webhook
```csharp
public class YourService
{
    private readonly IWebhookPublisher _webhookPublisher;

    public YourService(IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async Task PublishWebhook()
    {
        await _webhookPublisher.PublishAsync(
            webhookName: "YourWebhook",
            data: new { /* webhook data */ }
        );
    }
}
```

3. Handle Webhook Events
```csharp
public class YourWebhookHandler : IWebhookHandler, ITransientDependency
{
    public async Task HandleWebhookAsync(WebhookPayload webhook)
    {
        // Handle webhook event
    }
}
```
