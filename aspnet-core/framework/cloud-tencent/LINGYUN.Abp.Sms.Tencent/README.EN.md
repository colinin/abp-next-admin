# LINGYUN.Abp.Sms.Tencent

Tencent Cloud SMS Service Module, integrating Tencent Cloud SMS service into ABP applications.

## Features

* Support for Tencent Cloud SMS sending functionality
* Multi-tenant configuration support
* Default signature and template configuration support
* Support for batch sending to multiple phone numbers
* SMS template parameter support
* Built-in error handling and logging

## Configuration Items

### Basic Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "Your Tencent Cloud SecretId", // Get from Tencent Cloud Console
      "SecretKey": "Your Tencent Cloud SecretKey", // Get from Tencent Cloud Console
      "DurationSecond": "600" // Session duration in seconds
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

## Basic Usage

1. Add module dependency
```csharp
[DependsOn(typeof(AbpSmsTencentModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure Tencent Cloud SMS service
```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "Your Tencent Cloud SecretId",
      "SecretKey": "Your Tencent Cloud SecretKey",
      "DurationSecond": "600"
    },
    "Abp.TencentCloud.Sms": {
      "AppId": "Your SMS Application ID",
      "DefaultSignName": "Your SMS Signature",
      "DefaultTemplateId": "Your Default Template ID"
    }
  }
}
```

3. SMS sending examples
```csharp
public class YourService
{
    private readonly ISmsSender _smsSender;

    public YourService(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    // Send using default signature and template
    public async Task SendSmsAsync(string phoneNumber, Dictionary<string, object> templateParams)
    {
        await _smsSender.SendAsync(
            phoneNumber,
            nameof(TencentCloudSmsSender),
            templateParams);
    }

    // Send using specified signature and template
    public async Task SendSmsAsync(
        string signName,
        string templateCode,
        string phoneNumber,
        Dictionary<string, object> templateParams)
    {
        await _smsSender.SendAsync(
            signName,
            templateCode,
            phoneNumber,
            templateParams);
    }
}
```

## Advanced Features

### Feature Switches

The module provides the following feature switches:

* TencentSms - Controls enabling/disabling of Tencent Cloud SMS service

### Error Handling

* Throws exception when all SMS sending fails
* Logs warning when partial SMS sending fails
* Supports viewing detailed failure information, including serial number, phone number, error code, and error message

## More Documentation

* [Tencent Cloud SMS Service](https://cloud.tencent.com/document/product/382)
* [SMS Console](https://console.cloud.tencent.com/smsv2)

[简体中文](./README.md)
