# LINGYUN.Abp.WebhooksManagement.Application

Webhook管理应用服务模块，提供Webhook管理的应用层实现。

[English](README.EN.md)

## 功能特性

* Webhook订阅管理
* Webhook组管理
* Webhook定义管理
* Webhook日志查询
* Webhook权限管理

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 权限定义

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

## 基本用法

1. 管理Webhook订阅
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
        // 创建订阅
        await _webhookSubscriptionAppService.CreateAsync(new WebhookSubscriptionCreateDto
        {
            WebhookUri = "https://your-webhook-endpoint",
            Webhooks = new[] { "YourWebhook" }
        });

        // 查询订阅
        var subscriptions = await _webhookSubscriptionAppService.GetListAsync(
            new WebhookSubscriptionGetListInput());
    }
}
```

2. 查询Webhook日志
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
