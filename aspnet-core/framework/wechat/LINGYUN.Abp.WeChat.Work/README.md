# LINGYUN.Abp.WeChat.Work

企业微信集成模块，提供企业微信应用开发所需的功能支持。

## 功能特性

* 企业微信身份验证
* 通讯录管理
* 消息推送
* 素材管理
* 客户联系
* 应用管理
* 身份验证
* 企业支付
* 电子发票

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WeChat": {
    "Work": {
      "CorpId": "",               // 企业ID
      "AgentId": 0,              // 应用ID
      "Secret": "",              // 应用密钥
      "Token": "",               // 消息Token
      "EncodingAESKey": "",      // 消息加密密钥
      "ApiUrl": "https://qyapi.weixin.qq.com" // API地址
    }
  }
}
```

## Settings配置

* `WeChat.Work.CorpId`: 企业ID
* `WeChat.Work.AgentId`: 应用ID
* `WeChat.Work.Secret`: 应用密钥
* `WeChat.Work.Token`: 消息Token
* `WeChat.Work.EncodingAESKey`: 消息加密密钥
* `WeChat.Work.ApiUrl`: API地址

## 更多文档

* [企业微信集成模块文档](README.EN.md)
