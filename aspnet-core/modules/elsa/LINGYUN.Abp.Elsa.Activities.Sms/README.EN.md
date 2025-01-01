# LINGYUN.Abp.Elsa.Activities.Sms

SMS activities integration module for Elsa workflow

## Features

* Provides **SendSms** activity for sending SMS messages
  * Support sending to multiple phone numbers
  * Support custom message properties
  * Support JavaScript, JSON, and Liquid syntax
  * Integration with ABP framework's `ISmsSender` interface

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesSmsModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "Sms": true    // Enable SMS activities
    }
}
```

## Activity Parameters

* **To**: List of recipient phone numbers
* **Message**: SMS content
* **Properties**: Additional properties (advanced option)

[中文文档](./README.md)
