# LINGYUN.Abp.Tencent

腾讯云服务基础模块，为其他腾讯云服务模块提供基础设施支持。

## 功能特性

* 提供腾讯云SDK客户端工厂，支持动态创建腾讯云各项服务的客户端
* 支持多租户配置
* 内置多语言本地化支持（中文和英文）
* 提供统一的腾讯云服务配置管理
* 支持特性（Feature）管理，可按需启用/禁用功能
* 支持区域（Region）本地化显示

## 配置项

### 基础配置

```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent": {
      "EndPoint": "ap-guangzhou", // 资源所在地域，默认为广州
      "SecretId": "您的腾讯云SecretId", // 从腾讯云控制台获取
      "SecretKey": "您的腾讯云SecretKey", // 从腾讯云控制台获取
      "DurationSecond": "600" // 会话持续时长，单位秒
    }
  }
}
```

### 连接配置

```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent.Connection": {
      "HttpMethod": "POST", // 请求方法，默认POST
      "Timeout": "60", // 连接超时时间，单位秒
      "WebProxy": "" // 代理服务器地址，可选
    }
  }
}
```

## 基本用法

1. 添加模块依赖
```csharp
[DependsOn(typeof(AbpTencentCloudModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 配置腾讯云服务
```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent": {
      "SecretId": "您的腾讯云SecretId",
      "SecretKey": "您的腾讯云SecretKey"
    }
  }
}
```

## 高级特性

### 特性管理

模块提供以下特性开关：

* TencentSms - 腾讯云短信服务
* TencentBlobStoring - 腾讯云对象存储服务
  * MaximumStreamSize - 单次上传文件流大小限制（MB）

### 多租户支持

所有配置均支持多租户配置，可以为不同租户配置不同的腾讯云服务参数。

## 更多文档

* [腾讯云API密钥管理](https://console.cloud.tencent.com/cam/capi)
* [腾讯云地域和可用区](https://cloud.tencent.com/document/product/213/6091)

[English](./README.EN.md)
