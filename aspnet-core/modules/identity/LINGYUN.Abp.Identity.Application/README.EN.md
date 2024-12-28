# LINGYUN.Abp.Identity.Application

Identity authentication application service module, providing implementation of identity authentication-related application services.

## Features

* Extends Volo.Abp.Identity.AbpIdentityApplicationModule module
* Provides implementation of identity authentication-related application services
* Integrates AutoMapper object mapping
* Supports user avatar URL extension property

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpIdentityDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Services

* IdentityUserAppService - User management service
* IdentityRoleAppService - Role management service
* IdentityClaimTypeAppService - Claim type management service
* IdentitySecurityLogAppService - Security log service
* IdentitySettingsAppService - Identity settings service
* ProfileAppService - User profile service

## Object Mapping

The module uses AutoMapper for object mapping, main mappings include:

* IdentityUser -> IdentityUserDto
* IdentityRole -> IdentityRoleDto
* IdentityClaimType -> IdentityClaimTypeDto
* IdentitySecurityLog -> IdentitySecurityLogDto

## Basic Usage

1. Using user management service
```csharp
public class YourService
{
    private readonly IIdentityUserAppService _userAppService;

    public YourService(IIdentityUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    public async Task<IdentityUserDto> GetUserAsync(Guid id)
    {
        return await _userAppService.GetAsync(id);
    }
}
```

2. Using role management service
```csharp
public class YourService
{
    private readonly IIdentityRoleAppService _roleAppService;

    public YourService(IIdentityRoleAppService roleAppService)
    {
        _roleAppService = roleAppService;
    }

    public async Task<IdentityRoleDto> GetRoleAsync(Guid id)
    {
        return await _roleAppService.GetAsync(id);
    }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
