# LINGYUN.Abp.WeChat.MiniProgram

微信小程序SDK集成模块，提供微信小程序开发所需的功能支持。

## 功能特性

* 微信小程序登录认证
* 小程序码生成
* 订阅消息发送
* 小程序数据统计
* 小程序直播
* 小程序支付集成
* 统一服务消息

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "WeChat": {
    "MiniProgram": {
      "AppId": "",                // 小程序AppId
      "AppSecret": "",           // 小程序AppSecret
      "Token": "",               // 小程序消息Token
      "EncodingAESKey": "",      // 小程序消息加密密钥
      "IsDebug": false,          // 是否开启调试模式
      "DefaultEnvironment": "release" // 默认环境，可选值：develop（开发版）、trial（体验版）、release（正式版）
    }
  }
}
```

#### 注意事项

在动态配置中有一个已知的问题: https://github.com/abpframework/abp/issues/6318  
因此必须要重建一个动态变更 AbpWeChatMiniProgramOptions 的方法，请使用AbpWeChatMiniProgramOptionsFactory.CreateAsync()

## Settings配置

* `WeChat.MiniProgram.AppId`: 小程序AppId
* `WeChat.MiniProgram.AppSecret`: 小程序AppSecret
* `WeChat.MiniProgram.Token`: 小程序消息Token
* `WeChat.MiniProgram.EncodingAESKey`: 小程序消息加密密钥
* `WeChat.MiniProgram.IsDebug`: 是否开启调试模式
* `WeChat.MiniProgram.DefaultEnvironment`: 默认环境

## 更多文档

* [微信小程序模块文档](README.EN.md)
