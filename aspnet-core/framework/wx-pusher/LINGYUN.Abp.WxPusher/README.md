# LINGYUN.Abp.WxPusher

集成WxPusher微信推送服务的ABP模块，实现WxPusher相关Api文档，拥有WxPusher开放能力。

[English](./README.EN.md)

详情见WxPusher文档: https://wxpusher.dingliqc.com/docs/#/  

## 功能特性

* 集成WxPusher API
* 支持消息推送到用户和Topic
* 支持多种消息类型（文本、HTML、Markdown）
* 支持消息发送限制和配额管理
* 支持二维码生成
* 支持用户订阅管理

## 安装

```bash
dotnet add package LINGYUN.Abp.WxPusher
```

## 模块引用

```csharp
[DependsOn(typeof(AbpWxPusherModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 配置

### Settings配置

* `WxPusher.Security.AppToken`: 应用的身份标志，拥有APP_TOKEN后可以给对应的应用的用户发送消息，请严格保密。

### Features功能

* `WxPusher`: WxPusher特性分组
* `WxPusher.Enable`: 全局启用WxPusher
* `WxPusher.Message.Enable`: 全局启用WxPusher消息通道
* `WxPusher.Message`: WxPusher消息推送
* `WxPusher.Message.SendLimit`: WxPusher消息推送限制次数
* `WxPusher.Message.SendLimitInterval`: WxPusher消息推送限制周期(天)

## 使用方式

### 发送消息

```csharp
public class YourService
{
    private readonly IWxPusherMessageSender _messageSender;

    public YourService(IWxPusherMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public async Task SendMessageAsync()
    {
        await _messageSender.SendAsync(
            content: "Hello, WxPusher!",
            summary: "消息摘要",
            contentType: MessageContentType.Text,
            topicIds: new List<int> { 1, 2 },  // 可选：发送到指定Topic
            uids: new List<string> { "UID1", "UID2" },  // 可选：发送到指定用户
            url: "https://example.com"  // 可选：点击消息跳转的URL
        );
    }
}
```

### 用户订阅

实现 `IWxPusherUserStore` 接口来管理用户订阅:

```csharp
public class YourWxPusherUserStore : IWxPusherUserStore
{
    public async Task<List<string>> GetBindUidsAsync(
        IEnumerable<Guid> userIds, 
        CancellationToken cancellationToken = default)
    {
        // 实现获取用户绑定的WxPusher UID的逻辑
    }

    public async Task<List<int>> GetSubscribeTopicsAsync(
        IEnumerable<Guid> userIds, 
        CancellationToken cancellationToken = default)
    {
        // 实现获取用户订阅的Topic列表的逻辑
    }
}
```

## 源码位置

[LINGYUN.Abp.WxPusher](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/wx-pusher/LINGYUN.Abp.WxPusher)
