# LINGYUN.Abp.WebhooksManagement.HttpApi

Webhook管理HTTP API模块，提供Webhook管理的REST API接口。

[English](README.EN.md)

## 功能特性

* Webhook订阅REST API
* Webhook组REST API
* Webhook定义REST API
* Webhook日志REST API
* 自动API路由
* API权限控制

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API路由

* /api/webhooks-management/subscriptions - Webhook订阅管理
* /api/webhooks-management/groups - Webhook组管理
* /api/webhooks-management/definitions - Webhook定义管理
* /api/webhooks-management/logs - Webhook日志查询

## 基本用法

1. 配置API路由
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

2. 调用API示例
```http
### 创建Webhook订阅
POST /api/webhooks-management/subscriptions
{
    "webhookUri": "https://your-webhook-endpoint",
    "webhooks": ["YourWebhook"]
}

### 查询Webhook订阅
GET /api/webhooks-management/subscriptions?maxResultCount=10&skipCount=0

### 查询Webhook日志
GET /api/webhooks-management/logs?maxResultCount=10&skipCount=0
```
