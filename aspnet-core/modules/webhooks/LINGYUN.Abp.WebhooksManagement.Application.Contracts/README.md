# LINGYUN.Abp.WebhooksManagement.Application.Contracts

Webhook管理应用服务契约模块，定义Webhook管理的应用服务接口和DTO。

[English](README.EN.md)

## 功能特性

* Webhook订阅服务接口
* Webhook组服务接口
* Webhook定义服务接口
* Webhook日志服务接口
* Webhook权限定义
* Webhook DTO定义

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 服务接口

* IWebhookSubscriptionAppService - Webhook订阅管理服务
* IWebhookGroupAppService - Webhook组管理服务
* IWebhookDefinitionAppService - Webhook定义管理服务
* IWebhookSendAttemptAppService - Webhook发送日志服务

## 基本用法

1. 实现Webhook订阅服务
```csharp
public class WebhookSubscriptionAppService : 
    ApplicationService,
    IWebhookSubscriptionAppService
{
    public async Task<WebhookSubscriptionDto> CreateAsync(
        WebhookSubscriptionCreateDto input)
    {
        // 实现创建订阅逻辑
    }

    public async Task<PagedResultDto<WebhookSubscriptionDto>> GetListAsync(
        WebhookSubscriptionGetListInput input)
    {
        // 实现查询订阅逻辑
    }
}
```

2. 使用Webhook DTO
```csharp
public class YourDto
{
    public WebhookSubscriptionDto Subscription { get; set; }
    public WebhookGroupDto Group { get; set; }
    public WebhookDefinitionDto Definition { get; set; }
    public WebhookSendAttemptDto SendAttempt { get; set; }
}
```
