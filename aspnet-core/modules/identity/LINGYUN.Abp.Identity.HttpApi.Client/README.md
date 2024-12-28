# LINGYUN.Abp.Identity.HttpApi.Client

身份认证HTTP API客户端模块，提供身份认证相关的HTTP API客户端代理。

## 功能特性

* 扩展Volo.Abp.Identity.AbpIdentityHttpApiClientModule模块
* 提供身份认证相关的HTTP API客户端代理
* 自动注册HTTP客户端代理服务

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpIdentityApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "RemoteServices": {
    "Identity": {
      "BaseUrl": "http://localhost:44388/"
    }
  }
}
```

## 客户端代理

* IIdentityUserAppService - 用户管理客户端代理
* IIdentityRoleAppService - 角色管理客户端代理
* IIdentityClaimTypeAppService - 声明类型管理客户端代理
* IIdentitySecurityLogAppService - 安全日志客户端代理
* IIdentitySettingsAppService - 身份认证设置客户端代理
* IProfileAppService - 用户配置文件客户端代理

## 基本用法

1. 配置远程服务
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    Configure<AbpRemoteServiceOptions>(options =>
    {
        options.RemoteServices.Default =
            new RemoteServiceConfiguration(configuration["RemoteServices:Identity:BaseUrl"]);
    });
}
```

2. 使用客户端代理
```csharp
public class YourService
{
    private readonly IIdentityUserAppService _userAppService;
    private readonly IIdentityRoleAppService _roleAppService;

    public YourService(
        IIdentityUserAppService userAppService,
        IIdentityRoleAppService roleAppService)
    {
        _userAppService = userAppService;
        _roleAppService = roleAppService;
    }

    public async Task<IdentityUserDto> GetUserAsync(Guid id)
    {
        return await _userAppService.GetAsync(id);
    }

    public async Task<IdentityRoleDto> GetRoleAsync(Guid id)
    {
        return await _roleAppService.GetAsync(id);
    }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [ABP HTTP API客户端代理文档](https://docs.abp.io/en/abp/latest/API/HTTP-Client-Proxies)
