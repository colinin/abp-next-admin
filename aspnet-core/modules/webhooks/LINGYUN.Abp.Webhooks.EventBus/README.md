# LINGYUN.Abp.Webhooks.EventBus

Webhook事件总线集成模块，提供与ABP事件总线的集成支持。

[English](README.EN.md)

## 功能特性

* 支持将Webhook事件发布到事件总线
* 与ABP事件总线无缝集成
* 支持分布式事件总线

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 定义Webhook事件处理器
```csharp
public class YourWebhookEventHandler : 
    IDistributedEventHandler<WebhookEventData>,
    ITransientDependency
{
    public async Task HandleEventAsync(WebhookEventData eventData)
    {
        // 处理webhook事件
    }
}
```

2. 发布Webhook事件
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
