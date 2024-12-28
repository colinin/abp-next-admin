# LINGYUN.Abp.WeChat.MiniProgram

WeChat Mini Program SDK integration module, providing necessary functionality support for WeChat Mini Program development.

## Features

* WeChat Mini Program login authentication
* Mini Program QR code generation
* Subscription message sending
* Mini Program data statistics
* Mini Program live streaming
* Mini Program payment integration
* Unified service messaging

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "MiniProgram": {
      "AppId": "",                // Mini Program AppId
      "AppSecret": "",           // Mini Program AppSecret
      "Token": "",               // Mini Program message Token
      "EncodingAESKey": "",      // Mini Program message encryption key
      "IsDebug": false,          // Whether to enable debug mode
      "DefaultEnvironment": "release" // Default environment, options: develop/trial/release
    }
  }
}
```

#### Important Note

There is a known issue with dynamic configuration: https://github.com/abpframework/abp/issues/6318  
Therefore, you must use AbpWeChatMiniProgramOptionsFactory.CreateAsync() to dynamically change AbpWeChatMiniProgramOptions.

## Settings Configuration

* `WeChat.MiniProgram.AppId`: Mini Program AppId
* `WeChat.MiniProgram.AppSecret`: Mini Program AppSecret
* `WeChat.MiniProgram.Token`: Mini Program message Token
* `WeChat.MiniProgram.EncodingAESKey`: Mini Program message encryption key
* `WeChat.MiniProgram.IsDebug`: Whether to enable debug mode
* `WeChat.MiniProgram.DefaultEnvironment`: Default environment

## More Documentation

* [Chinese Documentation](README.md)
