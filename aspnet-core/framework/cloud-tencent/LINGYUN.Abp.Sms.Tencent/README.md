# LINGYUN.Abp.Sms.Tencent

腾讯云短信服务模块，集成腾讯云短信服务到ABP应用程序。

## 功能特性

* 支持腾讯云短信服务的发送功能
* 支持多租户配置
* 支持默认签名和模板配置
* 支持多手机号批量发送
* 支持短信模板参数传递
* 内置错误处理和日志记录

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

### 短信服务配置

```json
{
  "Settings": {
    "Abp.TencentCloud.Sms": {
      "AppId": "", // 短信应用ID，在短信控制台添加应用后生成
      "DefaultSignName": "", // 默认短信签名
      "DefaultTemplateId": "" // 默认短信模板ID
    }
  }
}
```

## 基本用法

1. 添加模块依赖
```csharp
[DependsOn(typeof(AbpSmsTencentModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 配置腾讯云短信服务
```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "您的腾讯云SecretId",
      "SecretKey": "您的腾讯云SecretKey"
    },
    "Abp.TencentCloud.Sms": {
      "AppId": "您的短信应用ID",
      "DefaultSignName": "您的短信签名",
      "DefaultTemplateId": "您的默认模板ID"
    }
  }
}
```

3. 发送短信示例
```csharp
public class YourService
{
    private readonly ISmsSender _smsSender;

    public YourService(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    // 使用默认签名和模板发送
    public async Task SendSmsAsync(string phoneNumber, Dictionary<string, object> templateParams)
    {
        await _smsSender.SendAsync(
            phoneNumber,
            nameof(TencentCloudSmsSender),
            templateParams);
    }

    // 使用指定签名和模板发送
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

## 高级特性

### 特性开关

模块提供以下特性开关：

* TencentSms - 控制腾讯云短信服务的启用/禁用

### 错误处理

* 当所有短信发送失败时，会抛出异常
* 当部分短信发送失败时，会记录警告日志
* 支持查看发送失败的详细信息，包括流水号、手机号、错误代码和错误信息

## 更多文档

* [腾讯云短信服务](https://cloud.tencent.com/document/product/382)
* [短信控制台](https://console.cloud.tencent.com/smsv2)

[English](./README.EN.md)
