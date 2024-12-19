# LINGYUN.Abp.Saas.HttpApi.Client

SaaS HTTP API client module, providing HTTP client proxy implementation for tenant and edition management.

## Features

* HTTP Client Proxy
  * HTTP client implementation of ITenantAppService
  * HTTP client implementation of IEditionAppService

## Usage

1. Install Module

```csharp
[DependsOn(typeof(AbpSaasHttpApiClientModule))]
public class YourModule : AbpModule
{
}
```

2. Configure Remote Service

```json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:44388/"
    }
  }
}
```

3. Inject and Use Services

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

## More

[简体中文](README.md)
