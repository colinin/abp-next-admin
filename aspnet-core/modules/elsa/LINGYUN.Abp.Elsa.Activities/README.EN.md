# LINGYUN.Abp.Elsa.Activities

Integration of commonly used Activities for Elsa workflow engine.

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json
```json
{
    "Elsa": {
        "BlobStoring": true,    // Enable blob storage activities
        "Emailing": true,       // Enable email activities
        "Notification": true,   // Enable notification activities
        "Sms": true,           // Enable SMS activities
        "IM": true,            // Enable instant messaging activities
        "PublishWebhook": true // Enable webhook publishing activities
    }
}
```

## Features

This module integrates several commonly used activities for the Elsa workflow engine:

* **Blob Storage Activities**: File storage and management operations
* **Email Activities**: Email sending and processing
* **Notification Activities**: System notification handling
* **SMS Activities**: SMS message sending
* **IM Activities**: Instant messaging operations
* **Webhook Activities**: Webhook publishing and handling

[中文文档](./README.md)
