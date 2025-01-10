# LINGYUN.Abp.WeChat.Official

WeChat Official Account SDK integration module, providing necessary functionality support for WeChat Official Account development.

## Features

* WeChat Official Account OAuth2.0 authentication
* Custom menu management
* Template message sending
* Media management
* User management
* Customer service messaging
* WeChat payment integration
* Message encryption/decryption
* Event handling

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatOfficialModule))]
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
      "AppId": "",                // Official Account AppId
      "AppSecret": "",           // Official Account AppSecret
      "Token": "",               // Official Account message Token
      "EncodingAESKey": "",      // Official Account message encryption key
      "IsSandBox": false,        // Whether in sandbox environment
      "Url": ""                  // Official Account server URL
    }
  }
}
```

#### Important Note

There is a known issue with dynamic configuration: https://github.com/abpframework/abp/issues/6318  
Therefore, you must use AbpWeChatOfficialOptionsFactory.CreateAsync() to dynamically change AbpWeChatOfficialOptions.

## Settings Configuration

* `WeChat.Official.AppId`: Official Account AppId
* `WeChat.Official.AppSecret`: Official Account AppSecret
* `WeChat.Official.Token`: Official Account message Token
* `WeChat.Official.EncodingAESKey`: Official Account message encryption key
* `WeChat.Official.IsSandBox`: Whether in sandbox environment
* `WeChat.Official.Url`: Official Account server URL

## More Documentation

* [Chinese Documentation](README.md)
