# LINGYUN.Abp.TextTemplating.HttpApi.Client

## Module Description

Text templating HTTP API client module, providing HTTP client proxy implementation for text template management.

### Base Modules

* LINGYUN.Abp.TextTemplating.Application.Contracts
* Volo.Abp.Http.Client

### Features

* Provides HTTP client proxies for text template management
  * TextTemplateDefinitionClientProxy - Template definition management client proxy
  * TextTemplateContentClientProxy - Template content management client proxy
* Implements the following application service interfaces
  * ITextTemplateDefinitionAppService
  * ITextTemplateContentAppService

### Configuration

* AbpTextTemplatingRemoteServiceConsts
  * RemoteServiceName - Remote service name (default: "AbpTextTemplating")

### How to Use

1. Add `AbpTextTemplatingHttpApiClientModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingHttpApiClientModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Configure remote service
        Configure<AbpRemoteServiceOptions>(options =>
        {
            options.RemoteServices.Default.BaseUrl = "http://localhost:44315/";
        });
    }
}
```

2. Inject and use client proxies

```csharp
public class YourService
{
    private readonly ITextTemplateDefinitionAppService _templateDefinitionAppService;

    public YourService(ITextTemplateDefinitionAppService templateDefinitionAppService)
    {
        _templateDefinitionAppService = templateDefinitionAppService;
    }

    public async Task ManageTemplateAsync()
    {
        // Get template definition list
        var templates = await _templateDefinitionAppService.GetListAsync(
            new TextTemplateDefinitionGetListInput());
    }
}
```

[查看中文](README.md)
