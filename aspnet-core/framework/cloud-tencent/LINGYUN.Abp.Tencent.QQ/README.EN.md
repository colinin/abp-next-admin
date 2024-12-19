# LINGYUN.Abp.Tencent.QQ

Tencent QQ Connect module, integrating QQ Connect service into ABP applications.

## Features

* Support for QQ Connect quick login
* Multi-tenant configuration support
* Provides QQ Connect client factory for dynamic client creation

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

### QQ Connect Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud.QQConnect": {
      "AppId": "", // QQ Connect AppId, get from QQ Connect Management Center
      "AppKey": "", // QQ Connect AppKey, get from QQ Connect Management Center
      "IsMobile": "false" // Whether to use mobile style, default is PC style
    }
  }
}
```

## Basic Usage

1. Add module dependency
```csharp
[DependsOn(typeof(AbpTencentQQModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure QQ Connect service

Refer to the configuration items description above for the corresponding configuration.

3. QQ Connect service usage example
```csharp
public class YourService
{
    private readonly TencentQQClientFactory _qqClientFactory;

    public YourService(TencentQQClientFactory qqClientFactory)
    {
        _qqClientFactory = qqClientFactory;
    }

    public async Task QQConnectAsync()
    {
        var qqClient = await _qqClientFactory.CreateAsync();
        // Use qqClient to call QQ Connect service APIs
        // For detailed API usage, please refer to QQ Connect development documentation
    }
}
```

## More Documentation

* [QQ Connect Open Platform](https://connect.qq.com/)
* [QQ Connect Development Documentation](https://wiki.connect.qq.com/)

[简体中文](./README.md)
