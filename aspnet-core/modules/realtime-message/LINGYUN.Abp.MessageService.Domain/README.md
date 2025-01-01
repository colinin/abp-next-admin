# LINGYUN.Abp.MessageService.Domain

消息服务领域层模块。

## 功能特性

* 实现消息服务的领域逻辑
* 集成对象扩展功能
* 集成缓存功能
* 集成通知功能
* 支持自动映射
* 支持多语言

## 依赖模块

* `AbpAutoMapperModule`
* `AbpCachingModule`
* `AbpNotificationsModule`
* [LINGYUN.Abp.MessageService.Domain.Shared](../LINGYUN.Abp.MessageService.Domain.Shared/README.md)

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.MessageService.Domain 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.MessageService.Domain
```

2. 添加 `AbpMessageServiceDomainModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpMessageServiceDomainModule))]
public class YourModule : AbpModule
{
}
```

## 更多

[English document](README.EN.md)
