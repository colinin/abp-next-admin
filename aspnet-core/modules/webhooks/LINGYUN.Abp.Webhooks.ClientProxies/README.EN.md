# LINGYUN.Abp.Webhooks.ClientProxies

Webhook client proxy module that provides proxy implementation for webhook clients.

[简体中文](README.md)

## Features

* Webhook client proxy
* HTTP client configuration
* Automatic retry mechanism
* Error handling

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksClientProxiesModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Webhooks": {
    "ClientProxies": {
      "RetryCount": 3,  // Number of retry attempts
      "RetryInterval": "00:00:05",  // Retry interval
      "HttpTimeout": "00:00:30"  // HTTP request timeout
    }
  }
}
```

## Basic Usage

1. Configure Client Proxy
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpWebhooksClientProxiesOptions>(options =>
    {
        options.RetryCount = 5;
        options.RetryInterval = TimeSpan.FromSeconds(10);
    });
}
```

2. Use Client Proxy
```csharp
public class YourService
{
    private readonly IWebhookClientProxy _webhookClientProxy;

    public YourService(IWebhookClientProxy webhookClientProxy)
    {
        _webhookClientProxy = webhookClientProxy;
    }

    public async Task SendWebhook()
    {
        await _webhookClientProxy.SendAsync(
            new WebhookSendArgs
            {
                WebhookUri = "https://your-webhook-endpoint",
                Data = new { /* webhook data */ }
            });
    }
}
```
