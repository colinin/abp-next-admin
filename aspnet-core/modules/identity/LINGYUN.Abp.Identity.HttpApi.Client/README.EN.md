# LINGYUN.Abp.Identity.HttpApi.Client

Identity authentication HTTP API client module, providing HTTP API client proxies for identity authentication.

## Features

* Extends Volo.Abp.Identity.AbpIdentityHttpApiClientModule module
* Provides HTTP API client proxies for identity authentication
* Automatically registers HTTP client proxy services

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpIdentityApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "RemoteServices": {
    "Identity": {
      "BaseUrl": "http://localhost:44388/"
    }
  }
}
```

## Client Proxies

* IIdentityUserAppService - User management client proxy
* IIdentityRoleAppService - Role management client proxy
* IIdentityClaimTypeAppService - Claim type management client proxy
* IIdentitySecurityLogAppService - Security log client proxy
* IIdentitySettingsAppService - Identity settings client proxy
* IProfileAppService - User profile client proxy

## Basic Usage

1. Configure remote services
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

2. Use client proxies
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

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [ABP HTTP API Client Proxies Documentation](https://docs.abp.io/en/abp/latest/API/HTTP-Client-Proxies)
