# LINGYUN.Abp.WeChat.Work.Handlers

企业微信消息处理模块，提供企业微信消息和事件处理的基础实现。

## 功能特性

* 文本消息处理
* 图片消息处理
* 语音消息处理
* 视频消息处理
* 位置消息处理
* 链接消息处理
* 事件消息处理
* 自定义消息处理器扩展

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkHandlersModule))]
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
      "Handlers": {
        "DefaultResponseType": "text",  // 默认响应类型：text/image/voice/video/news
        "DefaultResponse": "",          // 默认响应内容
        "EnableDefaultResponse": true   // 是否启用默认响应
      }
    }
  }
}
```

## 自定义处理器

要实现自定义消息处理器，需要继承相应的基类：

* 文本消息：`WeChatWorkTextMessageHandlerBase`
* 图片消息：`WeChatWorkImageMessageHandlerBase`
* 语音消息：`WeChatWorkVoiceMessageHandlerBase`
* 视频消息：`WeChatWorkVideoMessageHandlerBase`
* 位置消息：`WeChatWorkLocationMessageHandlerBase`
* 链接消息：`WeChatWorkLinkMessageHandlerBase`
* 事件消息：`WeChatWorkEventMessageHandlerBase`

## 事件处理器

内置以下事件处理器：

* 应用菜单事件处理器
* 进入应用事件处理器
* 上报地理位置事件处理器
* 异步任务完成事件处理器
* 外部联系人事件处理器
* 通讯录变更事件处理器

## 更多文档

* [企业微信消息处理模块文档](README.EN.md)
