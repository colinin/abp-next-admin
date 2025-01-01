# LINGYUN.Abp.WeChat.SettingManagement

微信设置管理模块，提供微信相关配置的管理功能。

## 功能特性

* 微信配置管理
* 公众号配置管理
* 小程序配置管理
* 企业微信配置管理
* 配置界面集成
* 多租户支持

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Settings配置

### 公众号配置

* `WeChat.Official.AppId`: 公众号AppId
* `WeChat.Official.AppSecret`: 公众号AppSecret
* `WeChat.Official.Token`: 公众号消息Token
* `WeChat.Official.EncodingAESKey`: 公众号消息加密密钥
* `WeChat.Official.IsSandBox`: 是否为沙箱环境
* `WeChat.Official.Url`: 公众号服务器URL

### 小程序配置

* `WeChat.MiniProgram.AppId`: 小程序AppId
* `WeChat.MiniProgram.AppSecret`: 小程序AppSecret
* `WeChat.MiniProgram.Token`: 小程序消息Token
* `WeChat.MiniProgram.EncodingAESKey`: 小程序消息加密密钥
* `WeChat.MiniProgram.IsDebug`: 是否开启调试模式
* `WeChat.MiniProgram.DefaultEnvironment`: 默认环境

### 企业微信配置

* `WeChat.Work.CorpId`: 企业ID
* `WeChat.Work.AgentId`: 应用ID
* `WeChat.Work.Secret`: 应用密钥
* `WeChat.Work.Token`: 消息Token
* `WeChat.Work.EncodingAESKey`: 消息加密密钥

## 权限

* `WeChat.Setting`: 微信设置管理
* `WeChat.Setting.Official`: 公众号设置管理
* `WeChat.Setting.MiniProgram`: 小程序设置管理
* `WeChat.Setting.Work`: 企业微信设置管理

## 更多文档

* [微信设置管理模块文档](README.EN.md)
