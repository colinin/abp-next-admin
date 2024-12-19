# LINGYUN.Abp.WeChat.Official.Handlers

微信公众号消息处理模块，提供微信公众号消息和事件处理的基础实现。

## 功能特性

* 文本消息处理
* 图片消息处理
* 语音消息处理
* 视频消息处理
* 小视频消息处理
* 地理位置消息处理
* 链接消息处理
* 关注/取消关注事件处理
* 扫描带参数二维码事件处理
* 上报地理位置事件处理
* 自定义菜单事件处理
* 模板消息事件处理

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatOfficialHandlersModule))]
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
      "Handlers": {
        "DefaultResponseType": "text",  // 默认响应类型：text/image/voice/video/music/news
        "DefaultResponse": "",          // 默认响应内容
        "EnableDefaultResponse": true   // 是否启用默认响应
      }
    }
  }
}
```

## 自定义处理器

要实现自定义消息处理器，需要继承相应的基类：

* 文本消息：`WeChatOfficialTextMessageHandlerBase`
* 图片消息：`WeChatOfficialImageMessageHandlerBase`
* 语音消息：`WeChatOfficialVoiceMessageHandlerBase`
* 视频消息：`WeChatOfficialVideoMessageHandlerBase`
* 事件消息：`WeChatOfficialEventMessageHandlerBase`

## 更多文档

* [微信公众号消息处理模块文档](README.EN.md)
