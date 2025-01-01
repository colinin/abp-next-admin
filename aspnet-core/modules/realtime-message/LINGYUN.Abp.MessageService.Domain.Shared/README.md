# LINGYUN.Abp.MessageService.Domain.Shared

消息服务领域共享层模块。

## 功能特性

* 定义消息服务的基础设施
* 提供多语言资源
* 定义异常本地化
* 定义错误代码

## 依赖模块

* `AbpLocalizationModule`

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.MessageService.Domain.Shared 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.MessageService.Domain.Shared
```

2. 添加 `AbpMessageServiceDomainSharedModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpMessageServiceDomainSharedModule))]
public class YourModule : AbpModule
{
}
```

## 更多

[English document](README.EN.md)
