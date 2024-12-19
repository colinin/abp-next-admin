# LINGYUN.Abp.Elsa.Activities.Webhooks

Elsa工作流的Webhook活动集成模块

## 功能

* 提供 **PublishWebhook** 活动用于发布Webhook
  * 支持发送数据到订阅者
  * 支持自定义HTTP头部
  * 支持JavaScript、JSON和Liquid语法
  * 集成ABP框架的 `IWebhookPublisher` 接口
  * 支持多租户

## 配置使用

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesWebhooksModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "PublishWebhook": true    // 启用Webhook活动
    }
}
```

## 活动参数

* **WebhooName**: Webhook的唯一名称
* **WebhookData**: 要发送的数据
* **SendExactSameData**: 是否直接发送原始数据（如果为true，将直接发送参数中的数据给客户端）
* **UseOnlyGivenHeaders**: 是否仅使用给定的头部（如果为true，将仅使用给定的头部；如果为false，给定的头部将添加到订阅中预定义的头部）
* **Headers**: 要发送的HTTP头部（高级选项）

[English](./README.EN.md)
