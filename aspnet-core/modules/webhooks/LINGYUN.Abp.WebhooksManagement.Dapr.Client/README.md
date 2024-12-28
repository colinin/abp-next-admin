# LINGYUN.Abp.WebhooksManagement.Dapr.Client

Webhook管理Dapr客户端集成模块，提供与Dapr服务调用构建块的集成支持。

[English](README.EN.md)

## 功能特性

* 支持通过Dapr服务调用访问Webhook管理服务
* 与Dapr服务调用构建块无缝集成
* 支持分布式服务调用

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksManagementDaprClientModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WebhooksManagement": {
    "Dapr": {
      "AppId": "webhooks-management",  // Webhook管理服务的Dapr应用ID
      "HttpEndpoint": "http://localhost:3500"  // Dapr sidecar HTTP端点
    }
  }
}
```

## 基本用法

1. 配置Dapr客户端
```csharp
Configure<AbpWebhooksManagementDaprClientOptions>(options =>
{
    options.AppId = "webhooks-management";
    options.HttpEndpoint = "http://localhost:3500";
});
```

2. 使用Webhook管理客户端
```csharp
public class YourService
{
    private readonly IWebhookSubscriptionAppService _webhookSubscriptionAppService;

    public YourService(IWebhookSubscriptionAppService webhookSubscriptionAppService)
    {
        _webhookSubscriptionAppService = webhookSubscriptionAppService;
    }

    public async Task SubscribeWebhook()
    {
        await _webhookSubscriptionAppService.SubscribeAsync(new WebhookSubscriptionCreateDto
        {
            WebhookUri = "https://your-webhook-endpoint",
            Webhooks = new[] { "YourWebhook" }
        });
    }
}
```
