# LINGYUN.Abp.Elsa.Activities.BlobStoring

Blob Storage Activities integration module for Elsa workflow

## Features

* Provides the following Blob Storage activities:
  * **BlobExists**: Check if a blob exists
  * **WriteBlob**: Write blob data
  * **ReadBlob**: Read blob data
  * **DeleteBlob**: Delete blob data

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesBlobStoringModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "BlobStoring": true    // Enable Blob Storage activities
    }
}
```

[中文文档](./README.md)
