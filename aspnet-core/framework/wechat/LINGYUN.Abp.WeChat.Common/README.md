# LINGYUN.Abp.WeChat.Common

微信通用模块，提供微信产品线共享的基础功能实现。

## 功能特性

* 统一的消息处理框架
* 统一的消息加解密机制
* 统一的事件处理机制
* 统一的API调用接口
* 统一的错误处理机制

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatCommonModule))]
public class YouProjectModule : AbpModule
{
}
```

## 配置项

```json
{
  "WeChat": {
    "Common": {
      "MessageHandlerFactory": {
        "MessageResolvers": [],           // 消息解析器列表
        "MessageHandlers": []             // 消息处理器列表
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
* 支持事件消息处理
* 支持自定义消息处理器扩展

## 更多文档

* [微信通用模块文档](README.EN.md)

### 更新日志 
