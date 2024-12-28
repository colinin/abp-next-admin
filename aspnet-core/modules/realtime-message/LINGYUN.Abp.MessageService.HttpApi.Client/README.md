# LINGYUN.Abp.MessageService.HttpApi.Client

消息服务 HTTP API 客户端模块。

## 功能特性

* 提供消息服务的 HTTP 客户端代理
* 自动注册 HTTP 客户端代理服务

## 依赖模块

* [LINGYUN.Abp.MessageService.Application.Contracts](../LINGYUN.Abp.MessageService.Application.Contracts/README.md)
* `AbpHttpClientModule`

## 配置使用

1. 首先，需要安装 LINGYUN.Abp.MessageService.HttpApi.Client 到你的项目中：

```bash
dotnet add package LINGYUN.Abp.MessageService.HttpApi.Client
```

2. 添加 `AbpMessageServiceHttpApiClientModule` 到你的模块的依赖列表：

```csharp
[DependsOn(typeof(AbpMessageServiceHttpApiClientModule))]
public class YourModule : AbpModule
{
}
```

3. 配置远程服务地址：

```json
{
  "RemoteServices": {
    "AbpMessageService": {
      "BaseUrl": "http://your-service-url"
    }
  }
}
```

## 更多

[English document](README.EN.md)
