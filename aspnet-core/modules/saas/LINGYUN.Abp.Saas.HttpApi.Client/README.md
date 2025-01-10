# LINGYUN.Abp.Saas.HttpApi.Client

SaaS HTTP API客户端模块，提供了租户和版本管理的HTTP客户端代理实现。

## 功能特性

* HTTP客户端代理
  * ITenantAppService的HTTP客户端实现
  * IEditionAppService的HTTP客户端实现

## 使用方式

1. 安装模块

```csharp
[DependsOn(typeof(AbpSaasHttpApiClientModule))]
public class YourModule : AbpModule
{
}
```

2. 配置远程服务

```json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:44388/"
    }
  }
}
```

3. 注入并使用服务

```csharp
public class YourService
{
    private readonly ITenantAppService _tenantAppService;
    private readonly IEditionAppService _editionAppService;

    public YourService(
        ITenantAppService tenantAppService,
        IEditionAppService editionAppService)
    {
        _tenantAppService = tenantAppService;
        _editionAppService = editionAppService;
    }
}
```

## 更多

[English](README.EN.md)
