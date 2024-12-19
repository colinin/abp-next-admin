# LINGYUN.Abp.WeChat.Official

微信公众号SDK集成模块，提供微信公众号开发所需的功能支持。

## 功能特性

* 微信公众号OAuth2.0认证
* 自定义菜单管理
* 模板消息发送
* 素材管理
* 用户管理
* 客服消息
* 微信支付集成
* 消息加解密
* 事件处理

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatOfficialModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WeChat": {
    "Official": {
      "AppId": "",                // 公众号AppId
      "AppSecret": "",           // 公众号AppSecret
      "Token": "",               // 公众号消息Token
      "EncodingAESKey": "",      // 公众号消息加密密钥
      "IsSandBox": false,        // 是否为沙箱环境
      "Url": ""                  // 公众号服务器URL
    }
  }
}
```

#### 注意事项

在动态配置中有一个已知的问题: https://github.com/abpframework/abp/issues/6318  
因此必须要重建一个动态变更 AbpWeChatOfficialOptions 的方法，请使用AbpWeChatOfficialOptionsFactory.CreateAsync()

## Settings配置

* `WeChat.Official.AppId`: 公众号AppId
* `WeChat.Official.AppSecret`: 公众号AppSecret
* `WeChat.Official.Token`: 公众号消息Token
* `WeChat.Official.EncodingAESKey`: 公众号消息加密密钥
* `WeChat.Official.IsSandBox`: 是否为沙箱环境
* `WeChat.Official.Url`: 公众号服务器URL

## 更多文档

* [微信公众号模块文档](README.EN.md)
