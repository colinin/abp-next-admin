# LINGYUN.Abp.Aliyun

Alibaba Cloud SDK integration module.

Reference: [Alibaba Cloud API Documentation](https://help.aliyun.com/document_detail/28763.html)

## Features

- Provides basic SDK integration for Alibaba Cloud services
- Supports Alibaba Cloud RAM (Resource Access Management) authentication
- Supports STS Token access
- Supports Alibaba Cloud SMS service
- Supports Alibaba Cloud Object Storage Service (OSS)
- Provides distributed cache support for optimizing high concurrency scenarios

## Module Reference

```csharp
[DependsOn(typeof(AbpAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Items

### Authentication Configuration

- **AliyunSettingNames.Authorization.RegionId**

  - Description: Alibaba Cloud service region
  - Type: Optional
  - Default value: default
  - Example: oss-cn-hangzhou

- **AliyunSettingNames.Authorization.AccessKeyId**

  - Description: AccessKey ID of Alibaba Cloud RAM account
  - Type: Required
  - How to get: Access Alibaba Cloud Console - Access Control

- **AliyunSettingNames.Authorization.AccessKeySecret**
  - Description: AccessKey Secret of RAM account
  - Type: Required
  - How to get: Access Alibaba Cloud Console - Access Control

### STS Token Configuration

- **AliyunSettingNames.Authorization.UseSecurityTokenService**

  - Description: Whether to use STS Token access
  - Type: Optional
  - Default value: false
  - Recommendation: Recommended to enable for improved security

- **AliyunSettingNames.Authorization.RamRoleArn**

  - Description: Alibaba Cloud RAM role ARN
  - Type: Required when STS Token is enabled
  - Format: acs:ram::$accountID:role/$roleName

- **AliyunSettingNames.Authorization.RoleSessionName**

  - Description: Custom token name
  - Type: Optional
  - Usage: For access auditing

- **AliyunSettingNames.Authorization.DurationSeconds**

  - Description: Token expiration time
  - Type: Optional
  - Default value: 3000
  - Unit: Seconds

- **AliyunSettingNames.Authorization.Policy**
  - Description: Permission policy
  - Type: Optional
  - Format: JSON string

### SMS Service Configuration

```json
{
  "Settings": {
    "Abp.Aliyun.Sms": {
      "Domain": "dysmsapi.aliyuncs.com", // API endpoint, default is dysmsapi.aliyuncs.com
      "Version": "2017-05-25", // API version, default is 2017-05-25
      "ActionName": "SendSms", // API method name, default is SendSms
      "DefaultSignName": "", // Default SMS signature
      "DefaultTemplateCode": "", // Default SMS template code
      "DefaultPhoneNumber": "", // Default phone number for receiving SMS
      "VisableErrorToClient": "false" // Whether to show error messages to client
    }
  }
}
```

## Available Regions

The module supports the following Alibaba Cloud regions:

- China Regions

  - North China 1 (Qingdao) - cn-qingdao
  - North China 2 (Beijing) - cn-beijing
  - North China 3 (Zhangjiakou) - cn-zhangjiakou
  - North China 5 (Hohhot) - cn-huhehaote
  - East China 1 (Hangzhou) - cn-hangzhou
  - East China 2 (Shanghai) - cn-shanghai
  - South China 1 (Shenzhen) - cn-shenzhen
  - South China 2 (Heyuan) - cn-heyuan
  - South China 3 (Guangzhou) - cn-guangzhou
  - Southwest 1 (Chengdu) - cn-chengdu

- Hong Kong and International Regions
  - Hong Kong - cn-hongkong
  - US (Silicon Valley) - us-west-1
  - US (Virginia) - us-east-1
  - Japan (Tokyo) - ap-northeast-1
  - South Korea (Seoul) - ap-northeast-2
  - Singapore - ap-southeast-1
  - Australia (Sydney) - ap-southeast-2
  - Malaysia (Kuala Lumpur) - ap-southeast-3
  - Indonesia (Jakarta) - ap-southeast-5
  - Philippines (Manila) - ap-southeast-6
  - Thailand (Bangkok) - ap-southeast-7
  - India (Mumbai) - ap-south-1
  - Germany (Frankfurt) - eu-central-1
  - UK (London) - eu-west-1
  - UAE (Dubai) - me-east-1

## Performance Optimization

- In high concurrency scenarios, it is recommended to enable distributed caching to improve performance
- When using STS Token, the token will be automatically cached until expiration
- It is recommended to set DurationSeconds reasonably to avoid frequent token refreshes

## Related Modules

- [LINGYUN.Abp.Aliyun.SettingManagement](../LINGYUN.Abp.Aliyun.SettingManagement/README.md) - Provides configuration management functionality
- [LINGYUN.Abp.Aliyun.Features](../LINGYUN.Abp.Aliyun.Features/README.md) - Provides feature management functionality

[查看中文文档](README.md)
