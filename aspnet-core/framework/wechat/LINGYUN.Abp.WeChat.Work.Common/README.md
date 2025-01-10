# LINGYUN.Abp.WeChat.Work.Common

企业微信通用模块，提供企业微信应用开发的基础功能实现。

## 功能特性

* 统一的消息处理框架
* 统一的消息加解密机制
* 统一的事件处理机制
* 统一的API调用接口
* 统一的错误处理机制
* AccessToken管理
* JsApi Ticket管理

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkCommonModule))]
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
      "Common": {
        "MessageHandlerFactory": {
          "MessageResolvers": [],           // 消息解析器列表
          "MessageHandlers": []             // 消息处理器列表
        },
        "AccessToken": {
          "CacheExpiration": 7200,         // AccessToken缓存时间（秒）
          "CacheKey": "WeChat:Work:AccessToken:{0}" // AccessToken缓存键模板
        },
        "JsApiTicket": {
          "CacheExpiration": 7200,         // JsApi Ticket缓存时间（秒）
          "CacheKey": "WeChat:Work:JsApiTicket:{0}" // JsApi Ticket缓存键模板
        }
      }
    }
  }
}
```

## 消息处理

* 支持文本消息处理
* 支持图片消息处理
* 支持语音消息处理
* 支持视频消息处理
* 支持位置消息处理
* 支持链接消息处理
* 支持事件消息处理

## 事件处理

* 支持应用菜单事件
* 支持进入应用事件
* 支持上报地理位置事件
* 支持异步任务完成事件
* 支持外部联系人事件
* 支持通讯录变更事件

## 更多文档

* [企业微信通用模块文档](README.EN.md)
