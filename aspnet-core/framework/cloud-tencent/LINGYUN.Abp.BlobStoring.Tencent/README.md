# LINGYUN.Abp.BlobStoring.Tencent

腾讯云对象存储（COS）模块，集成腾讯云对象存储服务到ABP BlobStoring系统。

## 功能特性

* 支持腾讯云对象存储服务
* 支持多租户配置
* 支持自动创建存储桶（Bucket）
* 支持存储桶防盗链配置
* 支持多区域配置
* 支持文件大小限制
* 支持按租户隔离存储

## 配置项说明

### 基础配置

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "您的腾讯云SecretId", // 从腾讯云控制台获取
      "SecretKey": "您的腾讯云SecretKey", // 从腾讯云控制台获取
      "DurationSecond": "600" // 会话持续时长，单位秒
    }
  }
}
```

### 对象存储配置

```json
{
  "Tencent": {
    "OSS": {
      "AppId": "", // 腾讯云AppId
      "Region": "", // 存储桶所在地域
      "BucketName": "", // 存储桶名称
      "CreateBucketIfNotExists": false, // 存储桶不存在时是否创建
      "CreateBucketReferer": [] // 创建存储桶时的防盗链设置
    }
  }
}
```

### 存储桶命名规范

* 仅支持小写英文字母和数字，即[a-z，0-9]、中划线"-"及其组合
* 不能以短划线（-）开头或结尾
* 存储桶名称的最大允许字符受到地域简称和APPID的字符数影响，组成的完整请求域名字符数总计最多60个字符
* 更多规范请参考[腾讯云存储桶命名规范](https://cloud.tencent.com/document/product/436/13312)

### 对象命名规范

* 不允许以正斜线/或者反斜线\\开头
* 对象键中不支持ASCII控制字符中的字符上(↑)，字符下(↓)，字符右(→)，字符左(←)
* 更多规范请参考[腾讯云对象命名规范](https://cloud.tencent.com/document/product/436/13324)

## 基本用法

1. 添加模块依赖
```csharp
[DependsOn(typeof(AbpBlobStoringTencentCloudModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 配置腾讯云对象存储
```csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<YourContainer>(container =>
    {
        container.UseTencentCloud(tencent =>
        {
            tencent.AppId = "您的腾讯云AppId";
            tencent.Region = "ap-guangzhou";
            tencent.BucketName = "your-bucket-name";
            tencent.CreateBucketIfNotExists = true;
            tencent.CreateBucketReferer = new List<string>
            {
                "*.example.com",
                "example.com"
            };
        });
    });
});
```

3. 使用BLOB存储
```csharp
public class YourService
{
    private readonly IBlobContainer<YourContainer> _blobContainer;

    public YourService(IBlobContainer<YourContainer> blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveBlobAsync(string name, Stream stream)
    {
        await _blobContainer.SaveAsync(name, stream);
    }

    public async Task<Stream> GetBlobAsync(string name)
    {
        return await _blobContainer.GetAsync(name);
    }
}
```

## 高级特性

### 多租户支持

模块支持按租户隔离存储，存储路径格式如下：
* 宿主：`host/{blobName}`
* 租户：`tenants/{tenantId}/{blobName}`

### 特性管理

模块提供以下特性开关：

* TencentBlobStoring - 控制腾讯云对象存储服务的启用/禁用
* TencentBlobStoringMaximumStreamSize - 控制上传文件的最大大小限制（MB）

## 更多文档

* [腾讯云对象存储](https://cloud.tencent.com/document/product/436)
* [腾讯云COS控制台](https://console.cloud.tencent.com/cos)
* [ABP BlobStoring系统](https://docs.abp.io/en/abp/latest/Blob-Storing)

[English](./README.EN.md)
