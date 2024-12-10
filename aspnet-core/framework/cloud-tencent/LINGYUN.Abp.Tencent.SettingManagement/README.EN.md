# LINGYUN.Abp.Tencent.SettingManagement

Tencent Cloud Service Setting Management Module, providing configuration management interface and API for Tencent Cloud services.

## Features

* Provides configuration management interface for Tencent Cloud services
* Supports global and tenant-level configuration management
* Supports configuration for the following Tencent Cloud services:
  * Basic configuration (keys, regions, etc.)
  * Connection configuration (HTTP method, timeout, proxy, etc.)
  * SMS service configuration (application ID, default signature, default template, etc.)
  * QQ Connect configuration (application ID, application key, etc.)
* Built-in permission management
* Multi-language localization support
* Support for all Tencent Cloud available regions

## Permissions

* `TencentCloud` - Tencent Cloud service permission group
  * `TencentCloud.Settings` - Configure Tencent Cloud service permission

## API Endpoints

### Get Global Configuration

```http
GET /api/setting-management/tencent/by-global
```

### Get Current Tenant Configuration

```http
GET /api/setting-management/tencent/by-current-tenant
```

## Basic Usage

1. Add module dependency
```csharp
[DependsOn(typeof(AbpTencentCloudSettingManagementModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure authorization
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpPermissionOptions>(options =>
    {
        options.GrantByDefault(TencentCloudSettingPermissionNames.Settings);
    });
}
```

## Configuration Items

### Basic Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "EndPoint": "ap-guangzhou", // Resource region, default is Guangzhou
      "SecretId": "Your Tencent Cloud SecretId", // Get from Tencent Cloud Console
      "SecretKey": "Your Tencent Cloud SecretKey", // Get from Tencent Cloud Console
      "DurationSecond": "600" // Session duration in seconds
    }
  }
}
```

### Connection Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud.Connection": {
      "HttpMethod": "POST", // Request method, default is POST
      "Timeout": "60", // Connection timeout in seconds
      "WebProxy": "", // Proxy server address, optional
      "EndPoint": "" // Specific service domain, required for financial zone services
    }
  }
}
```

### SMS Service Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud.Sms": {
      "AppId": "", // SMS application ID, generated after adding application in SMS console
      "DefaultSignName": "", // Default SMS signature
      "DefaultTemplateId": "" // Default SMS template ID
    }
  }
}
```

### QQ Connect Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud.QQConnect": {
      "AppId": "", // QQ Connect application ID, get from QQ Connect Management Center
      "AppKey": "", // QQ Connect application key, get from QQ Connect Management Center
      "IsMobile": "false" // Whether to use mobile style, default is PC style
    }
  }
}
```

## Available Regions

The module supports the following Tencent Cloud regions:

* China Regions
  * North China (Beijing) - ap-beijing
  * Southwest China (Chengdu) - ap-chengdu
  * Southwest China (Chongqing) - ap-chongqing
  * South China (Guangzhou) - ap-guangzhou
  * Hong Kong/Macao/Taiwan (Hong Kong, China) - ap-hongkong
  * East China (Nanjing) - ap-nanjing
  * East China (Shanghai) - ap-shanghai
  * East China (Shanghai Financial) - ap-shanghai-fsi
  * South China (Shenzhen Financial) - ap-shenzhen-fsi

* Asia Pacific
  * Bangkok - ap-bangkok
  * Jakarta - ap-jakarta
  * Mumbai - ap-mumbai
  * Seoul - ap-seoul
  * Singapore - ap-singapore
  * Tokyo - ap-tokyo

* Europe
  * Frankfurt - eu-frankfurt
  * Moscow - eu-moscow

* North America
  * Virginia - na-ashburn
  * Silicon Valley - na-siliconvalley
  * Toronto - na-toronto

## More Documentation

* [Tencent Cloud API Key Management](https://console.cloud.tencent.com/cam/capi)
* [Tencent Cloud Regions and Availability Zones](https://cloud.tencent.com/document/product/213/6091)

[简体中文](./README.md)
