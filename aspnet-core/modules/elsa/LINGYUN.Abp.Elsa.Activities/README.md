# LINGYUN.Abp.Elsa.Activities

常用Activity集成  

## 配置使用

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
            "BlobStoring": true,
            "Emailing": true,
            "Notification": true,
            "Sms": true,
            "IM": true,
            "PublishWebhook": true
        }
    }

```