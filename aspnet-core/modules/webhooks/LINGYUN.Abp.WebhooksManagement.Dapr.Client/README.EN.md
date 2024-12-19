# LINGYUN.Abp.WebhooksManagement.Dapr.Client

Webhook management Dapr client integration module that provides integration with Dapr service invocation building block.

[简体中文](README.md)

## Features

* Support for accessing webhook management service through Dapr service invocation
* Seamless integration with Dapr service invocation building block
* Support for distributed service invocation

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksManagementDaprClientModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WebhooksManagement": {
    "Dapr": {
      "AppId": "webhooks-management",  // Dapr application ID for webhook management service
      "HttpEndpoint": "http://localhost:3500"  // Dapr sidecar HTTP endpoint
    }
  }
}
```

## Basic Usage

1. Configure Dapr Client
```csharp
Configure<AbpWebhooksManagementDaprClientOptions>(options =>
{
    options.AppId = "webhooks-management";
    options.HttpEndpoint = "http://localhost:3500";
});
```

2. Use Webhook Management Client
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
