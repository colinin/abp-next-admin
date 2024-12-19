# LINGYUN.Abp.Elsa.Activities.Webhooks

Webhook activities integration module for Elsa workflow

## Features

* Provides **PublishWebhook** activity for publishing webhooks
  * Support sending data to subscribers
  * Support custom HTTP headers
  * Support JavaScript, JSON, and Liquid syntax
  * Integration with ABP framework's `IWebhookPublisher` interface
  * Support multi-tenancy

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesWebhooksModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "PublishWebhook": true    // Enable webhook activities
    }
}
```

## Activity Parameters

* **WebhooName**: Unique name of the webhook
* **WebhookData**: Data to send
* **SendExactSameData**: If true, it sends the exact same data as the parameter to clients
* **UseOnlyGivenHeaders**: If true, webhook will only contain given headers; if false, given headers will be added to predefined headers in subscription
* **Headers**: HTTP headers to be sent (advanced option)

[中文文档](./README.md)
