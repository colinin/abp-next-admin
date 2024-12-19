# LINGYUN.Abp.WebhooksManagement.Application

Webhook management application service module that provides application layer implementation for webhook management.

[简体中文](README.md)

## Features

* Webhook subscription management
* Webhook group management
* Webhook definition management
* Webhook log querying
* Webhook permission management

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Permission Definitions

* WebhooksManagement.Webhooks
  * WebhooksManagement.Webhooks.Create
  * WebhooksManagement.Webhooks.Update
  * WebhooksManagement.Webhooks.Delete
  * WebhooksManagement.Webhooks.ManagePermissions
* WebhooksManagement.Groups
  * WebhooksManagement.Groups.Create
  * WebhooksManagement.Groups.Update
  * WebhooksManagement.Groups.Delete
* WebhooksManagement.Subscriptions
  * WebhooksManagement.Subscriptions.Create
  * WebhooksManagement.Subscriptions.Update
  * WebhooksManagement.Subscriptions.Delete
* WebhooksManagement.Logs
  * WebhooksManagement.Logs.Default

## Basic Usage

1. Manage Webhook Subscriptions
```csharp
public class YourService
{
    private readonly IWebhookSubscriptionAppService _webhookSubscriptionAppService;

    public YourService(IWebhookSubscriptionAppService webhookSubscriptionAppService)
    {
        _webhookSubscriptionAppService = webhookSubscriptionAppService;
    }

    public async Task ManageSubscription()
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

2. Query Webhook Logs
```csharp
public class YourService
{
    private readonly IWebhookSendAttemptAppService _webhookSendAttemptAppService;

    public YourService(IWebhookSendAttemptAppService webhookSendAttemptAppService)
    {
        _webhookSendAttemptAppService = webhookSendAttemptAppService;
    }

    public async Task QueryLogs()
    {
        var logs = await _webhookSendAttemptAppService.GetListAsync(
            new WebhookSendAttemptGetListInput());
    }
}
```
