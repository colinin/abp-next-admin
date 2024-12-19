# LINGYUN.Abp.Elsa.Activities.BlobStoring

Elsa工作流的Blob存储活动集成模块

## 功能

* 提供以下Blob存储活动:
  * **BlobExists**: 检查Blob是否存在
  * **WriteBlob**: 写入Blob数据
  * **ReadBlob**: 读取Blob数据
  * **DeleteBlob**: 删除Blob数据

## 配置使用

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
        "BlobStoring": true    // 启用Blob存储活动
    }
}
```

[English](./README.EN.md)
