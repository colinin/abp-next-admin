# LINGYUN.Abp.BlobStoring.Aliyun

abp框架对象存储提供者**IBlobProvider**的阿里云实现

## 配置使用

模块按需引用，需要引用阿里云公共基础认证模块

事先定义**appsettings.json**文件

```json
{
  "Aliyun": {
    "Auth": {
      "AccessKeyId": "你的阿里云访问标识",
      "AccessKeySecret": "你的阿里云访问密钥"
    },
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