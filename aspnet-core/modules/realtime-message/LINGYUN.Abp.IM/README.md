# LINGYUN.Abp.IM

即时通讯模块的基础模块。

## 功能特性

* 提供即时通讯基础设施
* 提供消息发送者提供程序接口
* 可扩展的消息发送者提供程序

## 配置使用

模块配置使用 `AbpIMOptions` 选项类:

```csharp
Configure<AbpIMOptions>(options =>
{
    // 添加自定义消息发送者提供程序
    options.Providers.Add<CustomMessageSenderProvider>();
});
```

## 更多

[English document](README.EN.md)
