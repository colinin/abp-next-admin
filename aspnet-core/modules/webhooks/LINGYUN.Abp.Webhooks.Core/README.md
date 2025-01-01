# LINGYUN.Abp.Webhooks.Core

Webhook核心模块，提供Webhook定义、配置和基础功能支持。

[English](README.EN.md)

## 功能特性

* Webhook定义管理
* 可配置的Webhook超时和重试机制
* 自动订阅失效保护
* 可自定义HTTP请求头
* 支持多Webhook提供者

## 模块引用

```csharp
[DependsOn(typeof(AbpWebhooksCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Webhooks": {
    "TimeoutDuration": "00:01:00",  // 默认超时时间，默认60秒
    "MaxSendAttemptCount": 5,  // 默认最大发送次数
    "IsAutomaticSubscriptionDeactivationEnabled": true,  // 是否在达到最大连续失败次数时自动取消订阅
    "MaxConsecutiveFailCountBeforeDeactivateSubscription": 15,  // 取消订阅前最大连续失败次数，默认为MaxSendAttemptCount * 3
    "DefaultAgentIdentifier": "Abp-Webhooks",  // 默认发送方标识
    "DefaultHttpHeaders": {  // 默认请求头
      "_AbpDontWrapResult": "true",
      "X-Requested-From": "abp-webhooks"
    }
  }
}
```

## 基本用法

1. 定义Webhook
```csharp
public class YourWebhookDefinitionProvider : WebhookDefinitionProvider
{
    public override void Define(IWebhookDefinitionContext context)
    {
        context.Add(
            new WebhookDefinition(
                name: "TestWebhook",
                displayName: L("DisplayName:TestWebhook"),
                description: L("Description:TestWebhook")
            )
        );
    }
}
```

2. 配置Webhook选项
```csharp
Configure<AbpWebhooksOptions>(options =>
{
    options.TimeoutDuration = TimeSpan.FromMinutes(2);
    options.MaxSendAttemptCount = 3;
    options.AddHeader("Custom-Header", "Value");
});
```
