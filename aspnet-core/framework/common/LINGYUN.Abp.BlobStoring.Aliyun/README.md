# LINGYUN.Abp.BlobStoring.Aliyun

abp框架对象存储提供者**IBlobProvider**的阿里云实现

## 配置使用

模块按需引用

事先定义**appsettings.json**文件

```json
{
  "Aliyun": {
    "OSS": {
      "BucketName": "你定义的BucketName",
      "Endpoint": "http://oss-cn-shanghai.aliyuncs.com",
      "CreateBucketIfNotExists": true
    }
  }
}

```

```csharp
[DependsOn(typeof(AbpBlobStoringAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```