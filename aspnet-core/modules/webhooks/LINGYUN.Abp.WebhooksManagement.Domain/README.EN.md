# LINGYUN.Abp.WebhooksManagement.Domain

Webhook management domain module that provides webhook storage and management functionality.

[简体中文](README.md)

## Features

* Support for storing static webhooks in database
* Support for dynamic webhook storage
* Webhook cache management
* Timestamp expiration mechanism

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksManagementDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WebhooksManagement": {
    "SaveStaticWebhooksToDatabase": true,  // Whether to save static webhooks to database, default true
    "IsDynamicWebhookStoreEnabled": false,  // Whether to enable dynamic webhook storage, default false
    "WebhooksCacheRefreshInterval": "00:00:30",  // Cache refresh interval, default 30 seconds
    "WebhooksCacheStampTimeOut": "00:02:00",  // Timestamp request timeout, default 2 minutes
    "WebhooksCacheStampExpiration": "00:30:00"  // Timestamp expiration time, default 30 minutes
  }
}
```

## Basic Usage

1. Configure Webhook Management Options
```csharp
Configure<WebhooksManagementOptions>(options =>
{
    options.SaveStaticWebhooksToDatabase = true;
    options.IsDynamicWebhookStoreEnabled = true;
    options.WebhooksCacheRefreshInterval = TimeSpan.FromMinutes(1);
});
```

2. Using Webhook Storage
```csharp
public class YourService
{
    private readonly IWebhookDefinitionManager _webhookDefinitionManager;

    public YourService(IWebhookDefinitionManager webhookDefinitionManager)
    {
        _webhookDefinitionManager = webhookDefinitionManager;
    }

    public async Task DoSomething()
    {
        var webhooks = await _webhookDefinitionManager.GetAllAsync();
        // Process webhooks
    }
}
```
