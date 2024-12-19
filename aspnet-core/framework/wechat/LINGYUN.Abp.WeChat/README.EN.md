# LINGYUN.Abp.WeChat

WeChat base module, providing fundamental functionality and configuration for WeChat application development.

## Features

* WeChat basic configuration management
* Unified WeChat API calling interface
* WeChat AccessToken management
* Unified error handling mechanism

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "BaseUrl": "https://api.weixin.qq.com",  // WeChat API base URL
    "DefaultTimeout": 30000,                  // Default timeout in milliseconds
    "RetryCount": 3,                         // Number of retry attempts
    "RetryMilliseconds": 1000                // Retry interval in milliseconds
  }
}
```

## Settings Configuration

* `WeChat.BaseUrl` : WeChat API base URL
* `WeChat.DefaultTimeout` : Default timeout
* `WeChat.RetryCount` : Number of retry attempts
* `WeChat.RetryMilliseconds` : Retry interval

## More Documentation

* [Chinese Documentation](README.md)
