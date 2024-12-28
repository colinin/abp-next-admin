# LINGYUN.Abp.Identity.Application.Contracts

Identity authentication application service contracts module, providing interface definitions for identity authentication-related application services.

## Features

* Extends Volo.Abp.Identity.AbpIdentityApplicationContractsModule module
* Provides interface definitions for identity authentication-related application services
* Provides DTO object definitions for identity authentication
* Integrates ABP authorization module

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpAuthorizationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Service Interfaces

* IIdentityUserAppService - User management service interface
* IIdentityRoleAppService - Role management service interface
* IIdentityClaimTypeAppService - Claim type management service interface
* IIdentitySecurityLogAppService - Security log service interface
* IIdentitySettingsAppService - Identity settings service interface
* IProfileAppService - User profile service interface

## DTO Objects

* IdentityUserDto - User DTO
* IdentityRoleDto - Role DTO
* IdentityClaimTypeDto - Claim type DTO
* IdentitySecurityLogDto - Security log DTO
* GetIdentityUsersInput - Get user list input DTO
* GetIdentityRolesInput - Get role list input DTO
* IdentityUserCreateDto - Create user DTO
* IdentityUserUpdateDto - Update user DTO
* IdentityRoleCreateDto - Create role DTO
* IdentityRoleUpdateDto - Update role DTO

## Basic Usage

1. Implement user management service interface
```csharp
public class YourIdentityUserAppService : IIdentityUserAppService
{
    public async Task<IdentityUserDto> GetAsync(Guid id)
    {
        // Implement logic to get user
    }

    public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        // Implement logic to get user list
    }
}
```

2. Implement role management service interface
```csharp
public class YourIdentityRoleAppService : IIdentityRoleAppService
{
    public async Task<IdentityRoleDto> GetAsync(Guid id)
    {
        // Implement logic to get role
    }

    public async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        // Implement logic to get role list
    }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
