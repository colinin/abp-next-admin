# LINGYUN.Abp.MessageService.Application.Contracts

消息服务应用层契约模块。

## 功能特性

* 定义消息服务的应用层接口
* 定义消息服务的 DTO
* 提供多语言资源
* 支持虚拟文件系统

## 依赖模块

* [LINGYUN.Abp.MessageService.Domain.Shared](../LINGYUN.Abp.MessageService.Domain.Shared/README.md)

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.MessageService.Application.Contracts 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.MessageService.Application.Contracts
```

2. 添加 `AbpMessageServiceApplicationContractsModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpMessageServiceApplicationContractsModule))]
public class YourModule : AbpModule
{
}
```

## 更多

[English document](README.EN.md)
