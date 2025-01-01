# LINGYUN.Abp.Webhooks.EventBus

Webhook event bus integration module that provides integration with the ABP event bus.

[简体中文](README.md)

## Features

* Support for publishing webhook events to the event bus
* Seamless integration with ABP event bus
* Support for distributed event bus

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Define Webhook Event Handler
```csharp
public class YourWebhookEventHandler : 
    IDistributedEventHandler<WebhookEventData>,
    ITransientDependency
{
    public async Task HandleEventAsync(WebhookEventData eventData)
    {
        // Handle webhook event
    }
}
```

2. Publish Webhook Event
```csharp
public class YourService
{
    private readonly IDistributedEventBus _eventBus;

    public YourService(IDistributedEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task PublishWebhook()
    {
        await _eventBus.PublishAsync(new WebhookEventData
        {
            WebhookName = "YourWebhook",
            Data = new { /* webhook data */ }
        });
    }
}
```
