# LINGYUN.Abp.Webhooks.ClientProxies

Webhook客户端代理模块，提供Webhook客户端的代理实现。

[English](README.EN.md)

## 功能特性

* Webhook客户端代理
* HTTP客户端配置
* 自动重试机制
* 错误处理

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksClientProxiesModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Webhooks": {
    "ClientProxies": {
      "RetryCount": 3,  // 重试次数
      "RetryInterval": "00:00:05",  // 重试间隔
      "HttpTimeout": "00:00:30"  // HTTP请求超时时间
    }
  }
}
```

## 基本用法

1. 配置客户端代理
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

2. 使用客户端代理
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
