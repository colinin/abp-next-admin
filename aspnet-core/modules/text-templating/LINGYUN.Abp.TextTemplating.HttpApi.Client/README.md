# LINGYUN.Abp.TextTemplating.HttpApi.Client

## 模块说明

文本模板 HTTP API 客户端模块，提供文本模板管理的 HTTP 客户端代理实现。

### 基础模块

* LINGYUN.Abp.TextTemplating.Application.Contracts
* Volo.Abp.Http.Client

### 功能定义

* 提供文本模板管理的 HTTP 客户端代理
  * TextTemplateDefinitionClientProxy - 模板定义管理客户端代理
  * TextTemplateContentClientProxy - 模板内容管理客户端代理
* 实现以下应用服务接口
  * ITextTemplateDefinitionAppService
  * ITextTemplateContentAppService

### 配置项

* AbpTextTemplatingRemoteServiceConsts
  * RemoteServiceName - 远程服务名称 (默认: "AbpTextTemplating")

### 如何使用

1. 添加 `AbpTextTemplatingHttpApiClientModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingHttpApiClientModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置远程服务
        Configure<AbpRemoteServiceOptions>(options =>
        {
            options.RemoteServices.Default.BaseUrl = "http://localhost:44315/";
        });
    }
}
```

2. 注入并使用客户端代理

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
        // 获取模板定义列表
        var templates = await _templateDefinitionAppService.GetListAsync(
            new TextTemplateDefinitionGetListInput());
    }
}
```

[查看英文](README.EN.md)
