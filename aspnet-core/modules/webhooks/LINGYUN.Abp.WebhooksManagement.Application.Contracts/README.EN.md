# LINGYUN.Abp.WebhooksManagement.Application.Contracts

Webhook management application service contracts module that defines application service interfaces and DTOs for webhook management.

[简体中文](README.md)

## Features

* Webhook subscription service interfaces
* Webhook group service interfaces
* Webhook definition service interfaces
* Webhook log service interfaces
* Webhook permission definitions
* Webhook DTO definitions

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Service Interfaces

* IWebhookSubscriptionAppService - Webhook subscription management service
* IWebhookGroupAppService - Webhook group management service
* IWebhookDefinitionAppService - Webhook definition management service
* IWebhookSendAttemptAppService - Webhook send attempt log service

## Basic Usage

1. Implement Webhook Subscription Service
```csharp
public class WebhookSubscriptionAppService : 
    ApplicationService,
    IWebhookSubscriptionAppService
{
    public async Task<WebhookSubscriptionDto> CreateAsync(
        WebhookSubscriptionCreateDto input)
    {
        // Implement subscription creation logic
    }

    public async Task<PagedResultDto<WebhookSubscriptionDto>> GetListAsync(
        WebhookSubscriptionGetListInput input)
    {
        // Implement subscription query logic
    }
}
```

2. Use Webhook DTOs
```csharp
public class YourDto
{
    public WebhookSubscriptionDto Subscription { get; set; }
    public WebhookGroupDto Group { get; set; }
    public WebhookDefinitionDto Definition { get; set; }
    public WebhookSendAttemptDto SendAttempt { get; set; }
}
```
