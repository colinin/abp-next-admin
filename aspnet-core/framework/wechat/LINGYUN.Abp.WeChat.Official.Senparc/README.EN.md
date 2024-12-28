# LINGYUN.Abp.WeChat.Official.Senparc

WeChat Official Account Senparc SDK integration module, providing integration support with Senparc.Weixin SDK.

## Features

* Senparc.Weixin SDK integration
* Compatible with Senparc message handling mechanism
* Compatible with Senparc event handling mechanism
* Support for Senparc configuration system
* Support for Senparc caching strategy

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatOfficialSenparcModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "Official": {
      "Senparc": {
        "IsEnabled": true,           // Whether to enable Senparc integration
        "Cache": {
          "Type": "Local",          // Cache type: Local/Redis/Memcached
          "Configuration": ""        // Cache configuration string
        }
      }
    }
  }
}
```

## Senparc Compatibility

This module maintains compatibility with Senparc.Weixin SDK, allowing you to:

* Use Senparc message handlers
* Use Senparc event handlers
* Use Senparc API calling methods
* Use Senparc caching mechanism
* Use Senparc extension features

## More Documentation

* [Chinese Documentation](README.md)
* [Senparc.Weixin SDK Documentation](https://github.com/JeffreySu/WeiXinMPSDK)
