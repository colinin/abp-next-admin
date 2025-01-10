# LINGYUN.Abp.WebhooksManagement.HttpApi.Client

Webhook管理HTTP API客户端模块，提供对Webhook管理HTTP API的动态代理客户端。

[English](README.EN.md)

## 功能特性

* 动态API客户端代理
* 自动HTTP客户端配置
* 支持远程服务调用
* 集成ABP动态C# API客户端

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksManagementHttpApiClientModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "RemoteServices": {
    "WebhooksManagement": {
      "BaseUrl": "http://localhost:44315/"  // Webhook管理服务的基础URL
    }
  }
}
```

## 基本用法

1. 配置远程服务
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

2. 使用HTTP客户端
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
