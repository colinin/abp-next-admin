# LINGYUN.Abp.Notifications.WeChat.MiniProgram

微信小程序消息通知模块，提供通过微信小程序向用户发送订阅消息的功能。

## 功能特性

* 支持微信小程序订阅消息发送
* 支持消息模板管理
* 支持动态消息数据配置
* 集成ABP通知系统

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsWeChatMiniProgramModule))]
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
      "Notifications": {
        "DefaultTemplateId": "",           // 默认模板ID
        "DefaultPage": "pages/index/index", // 默认跳转页面
        "DefaultState": "formal",          // 默认跳转小程序类型，developer为开发版；trial为体验版；formal为正式版
        "DefaultLang": "zh_CN"             // 默认语言
      }
    }
  }
}
```

## 更多文档

* [微信小程序消息通知文档](README.EN.md)
