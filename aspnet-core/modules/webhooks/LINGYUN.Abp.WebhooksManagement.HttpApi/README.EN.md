# LINGYUN.Abp.WebhooksManagement.HttpApi

Webhook management HTTP API module that provides REST API interfaces for webhook management.

[简体中文](README.md)

## Features

* Webhook subscription REST API
* Webhook group REST API
* Webhook definition REST API
* Webhook log REST API
* Automatic API routing
* API permission control

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Routes

* /api/webhooks-management/subscriptions - Webhook subscription management
* /api/webhooks-management/groups - Webhook group management
* /api/webhooks-management/definitions - Webhook definition management
* /api/webhooks-management/logs - Webhook log querying

## Basic Usage

1. Configure API Routing
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        options.ConventionalControllers.Create(
            typeof(AbpWebhooksManagementHttpApiModule).Assembly,
            opts =>
            {
                opts.RootPath = "webhooks-management";
            });
    });
}
```

2. API Call Examples
```http
### Create Webhook Subscription
POST /api/webhooks-management/subscriptions
{
    "webhookUri": "https://your-webhook-endpoint",
    "webhooks": ["YourWebhook"]
}

### Query Webhook Subscriptions
GET /api/webhooks-management/subscriptions?maxResultCount=10&skipCount=0

### Query Webhook Logs
GET /api/webhooks-management/logs?maxResultCount=10&skipCount=0
```
