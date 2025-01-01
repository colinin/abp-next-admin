# LINGYUN.Abp.MessageService.HttpApi

消息服务 HTTP API 模块。

## 功能特性

* 提供消息服务的 HTTP API 接口
* 支持 MVC 数据注解本地化
* 自动注册 API 控制器

## 依赖模块

* [LINGYUN.Abp.MessageService.Application.Contracts](../LINGYUN.Abp.MessageService.Application.Contracts/README.md)
* `AbpAspNetCoreMvcModule`

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.MessageService.HttpApi 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.MessageService.HttpApi
```

2. 添加 `AbpMessageServiceHttpApiModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpMessageServiceHttpApiModule))]
public class YourModule : AbpModule
{
}
```

## 更多

[English document](README.EN.md)
