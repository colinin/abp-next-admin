# LINGYUN.Abp.Tencent

Tencent Cloud Service Base Module, providing infrastructure support for other Tencent Cloud service modules.

## Features

* Provides Tencent Cloud SDK client factory, supporting dynamic creation of clients for various Tencent Cloud services
* Multi-tenant configuration support
* Built-in localization support (Chinese and English)
* Unified Tencent Cloud service configuration management
* Feature management support, enabling/disabling functionalities as needed
* Region localization display support

## Configuration

### Basic Settings

```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent": {
      "EndPoint": "ap-guangzhou", // Resource region, default is Guangzhou
      "SecretId": "Your Tencent Cloud SecretId", // Get from Tencent Cloud Console
      "SecretKey": "Your Tencent Cloud SecretKey", // Get from Tencent Cloud Console
      "DurationSecond": "600" // Session duration in seconds
    }
  }
}
```

### Connection Settings

```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent.Connection": {
      "HttpMethod": "POST", // Request method, default is POST
      "Timeout": "60", // Connection timeout in seconds
      "WebProxy": "" // Proxy server address, optional
    }
  }
}
```

## Basic Usage

1. Add module dependency
```csharp
[DependsOn(typeof(AbpTencentCloudModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure Tencent Cloud service
```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent": {
      "SecretId": "Your Tencent Cloud SecretId",
      "SecretKey": "Your Tencent Cloud SecretKey"
    }
  }
}
```

## Advanced Features

### Feature Management

The module provides the following feature switches:

* TencentSms - Tencent Cloud SMS service
* TencentBlobStoring - Tencent Cloud Object Storage service
  * MaximumStreamSize - Maximum file stream size limit for single upload (MB)

### Multi-tenant Support

All configurations support multi-tenant settings, allowing different Tencent Cloud service parameters for different tenants.

## More Documentation

* [Tencent Cloud API Key Management](https://console.cloud.tencent.com/cam/capi)
* [Tencent Cloud Regions and Availability Zones](https://cloud.tencent.com/document/product/213/6091)

[简体中文](./README.md)
