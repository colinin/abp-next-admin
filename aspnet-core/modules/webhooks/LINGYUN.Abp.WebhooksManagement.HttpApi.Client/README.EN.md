# LINGYUN.Abp.WebhooksManagement.HttpApi.Client

Webhook management HTTP API client module that provides dynamic proxy client for webhook management HTTP API.

[简体中文](README.md)

## Features

* Dynamic API client proxy
* Automatic HTTP client configuration
* Support for remote service calls
* Integration with ABP dynamic C# API client

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksManagementHttpApiClientModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "RemoteServices": {
    "WebhooksManagement": {
      "BaseUrl": "http://localhost:44315/"  // Base URL for webhook management service
    }
  }
}
```

## Basic Usage

1. Configure Remote Service
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();
    
    Configure<AbpRemoteServiceOptions>(options =>
    {
        options.RemoteServices.Default = new RemoteServiceConfiguration(
            configuration["RemoteServices:WebhooksManagement:BaseUrl"]);
    });
}
```

2. Use HTTP Client
```csharp
public class YourService
{
    private readonly IWebhookSubscriptionAppService _webhookSubscriptionAppService;

    public YourService(IWebhookSubscriptionAppService webhookSubscriptionAppService)
    {
        _webhookSubscriptionAppService = webhookSubscriptionAppService;
    }

    public async Task CallRemoteApi()
    {
        // Create subscription
        await _webhookSubscriptionAppService.CreateAsync(new WebhookSubscriptionCreateDto
        {
            WebhookUri = "https://your-webhook-endpoint",
            Webhooks = new[] { "YourWebhook" }
        });

        // Query subscriptions
        var subscriptions = await _webhookSubscriptionAppService.GetListAsync(
            new WebhookSubscriptionGetListInput());
    }
}
```
