# LINGYUN.Abp.Notifications.WeChat.Work

企业微信应用消息通知模块，提供通过企业微信应用向用户发送消息的功能。

## 功能特性

* 支持企业微信应用消息发送
* 支持文本消息
* 支持图片消息
* 支持语音消息
* 支持视频消息
* 支持文件消息
* 支持图文消息
* 支持模板卡片消息
* 集成ABP通知系统

## 模块引用

```csharp
[DependsOn(typeof(AbpNotificationsWeChatWorkModule))]
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
      "Notifications": {
        "DefaultAgentId": 0,            // 默认应用ID
        "DefaultToParty": "",           // 默认接收消息的部门ID列表
        "DefaultToTag": "",             // 默认接收消息的标签ID列表
        "DefaultSafe": 0,               // 默认表示是否是保密消息，0表示可对外分享，1表示不能分享且内容显示水印
        "DefaultEnableIdTrans": 0,      // 默认表示是否开启id转译，0表示否，1表示是
        "DefaultEnableDuplicateCheck": 0, // 默认表示是否开启重复消息检查，0表示否，1表示是
        "DefaultDuplicateCheckInterval": 1800 // 默认表示重复消息检查的时间间隔，默认1800秒
      }
    }
  }
}
```

## 更多文档

* [企业微信应用消息通知文档](README.EN.md)
