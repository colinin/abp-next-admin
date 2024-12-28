# LINGYUN.Abp.Webhooks.Core

Core webhook module that provides webhook definition, configuration and basic functionality support.

[简体中文](README.md)

## Features

* Webhook definition management
* Configurable webhook timeout and retry mechanism
* Automatic subscription deactivation protection
* Customizable HTTP headers
* Support for multiple webhook providers

## Module Dependencies

```csharp
[DependsOn(typeof(AbpWebhooksCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Webhooks": {
    "TimeoutDuration": "00:01:00",  // Default timeout duration, 60 seconds by default
    "MaxSendAttemptCount": 5,  // Maximum number of send attempts
    "IsAutomaticSubscriptionDeactivationEnabled": true,  // Whether to automatically deactivate subscription when reaching maximum consecutive failures
    "MaxConsecutiveFailCountBeforeDeactivateSubscription": 15,  // Maximum consecutive failures before subscription deactivation, default is MaxSendAttemptCount * 3
    "DefaultAgentIdentifier": "Abp-Webhooks",  // Default sender identifier
    "DefaultHttpHeaders": {  // Default HTTP headers
      "_AbpDontWrapResult": "true",
      "X-Requested-From": "abp-webhooks"
    }
  }
}
```

## Basic Usage

1. Define a Webhook
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

2. Configure Webhook Options
```csharp
Configure<AbpWebhooksOptions>(options =>
{
    options.TimeoutDuration = TimeSpan.FromMinutes(2);
    options.MaxSendAttemptCount = 3;
    options.AddHeader("Custom-Header", "Value");
});
```
