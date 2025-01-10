# LINGYUN.Abp.WebhooksManagement.Domain

Webhook管理领域模块，提供Webhook的存储和管理功能。

[English](README.EN.md)

## 功能特性

* 支持静态Webhook存储到数据库
* 支持动态Webhook存储
* Webhook缓存管理
* 时间戳过期机制

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksManagementDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WebhooksManagement": {
    "SaveStaticWebhooksToDatabase": true,  // 是否保存静态Webhook到数据库，默认true
    "IsDynamicWebhookStoreEnabled": false,  // 是否启用动态Webhook存储，默认false
    "WebhooksCacheRefreshInterval": "00:00:30",  // 缓存刷新时间，默认30秒
    "WebhooksCacheStampTimeOut": "00:02:00",  // 申请时间戳超时时间，默认2分钟
    "WebhooksCacheStampExpiration": "00:30:00"  // 时间戳过期时间，默认30分钟
  }
}
```

## 基本用法

1. 配置Webhook管理选项
```csharp
Configure<WebhooksManagementOptions>(options =>
{
    options.SaveStaticWebhooksToDatabase = true;
    options.IsDynamicWebhookStoreEnabled = true;
    options.WebhooksCacheRefreshInterval = TimeSpan.FromMinutes(1);
});
```

2. 使用Webhook存储
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
        // 处理webhook
    }
}
```
