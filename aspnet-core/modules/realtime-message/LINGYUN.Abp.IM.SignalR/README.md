# LINGYUN.Abp.IM.SignalR

基于 SignalR 实现的即时通讯模块。

## 功能特性

* 基于 SignalR 实现的消息发送者提供程序
* 集成 ABP SignalR 模块
* 多语言支持

## 依赖模块

* [LINGYUN.Abp.IM](../LINGYUN.Abp.IM/README.md)
* `AbpAspNetCoreSignalRModule`

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.IM.SignalR 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.IM.SignalR
```

2. 添加 `AbpIMSignalRModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpIMSignalRModule))]
public class YourModule : AbpModule
{
}
```

## 更多

[English document](README.EN.md)
