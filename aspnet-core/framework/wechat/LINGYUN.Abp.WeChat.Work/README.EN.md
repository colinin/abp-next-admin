# LINGYUN.Abp.WeChat.Work

WeChat Work (Enterprise WeChat) integration module, providing necessary functionality support for WeChat Work application development.

## Features

* WeChat Work authentication
* Contact management
* Message pushing
* Media management
* Customer contact
* Application management
* Authentication
* Enterprise payment
* Electronic invoice

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "WeChat": {
    "Work": {
      "CorpId": "",               // Enterprise ID
      "AgentId": 0,              // Application ID
      "Secret": "",              // Application secret
      "Token": "",               // Message Token
      "EncodingAESKey": "",      // Message encryption key
      "ApiUrl": "https://qyapi.weixin.qq.com" // API URL
    }
  }
}
```

## Settings Configuration

* `WeChat.Work.CorpId`: Enterprise ID
* `WeChat.Work.AgentId`: Application ID
* `WeChat.Work.Secret`: Application secret
* `WeChat.Work.Token`: Message Token
* `WeChat.Work.EncodingAESKey`: Message encryption key
* `WeChat.Work.ApiUrl`: API URL

## More Documentation

* [Chinese Documentation](README.md)
