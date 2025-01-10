# LINGYUN.Abp.Webhooks

Webhook基础模块，提供Webhook的基本定义和功能。

[English](README.EN.md)

## 功能特性

* 基本Webhook定义
* Webhook发布和订阅
* Webhook事件数据处理
* Webhook配置管理

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 定义Webhook
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

2. 发布Webhook
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

3. 处理Webhook事件
```csharp
public class YourWebhookHandler : IWebhookHandler, ITransientDependency
{
    public async Task HandleWebhookAsync(WebhookPayload webhook)
    {
        // 处理webhook事件
    }
}
```
