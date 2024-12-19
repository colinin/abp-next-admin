# LINGYUN.Abp.Identity.Application

身份认证应用服务模块，提供身份认证相关的应用服务实现。

## 功能特性

* 扩展Volo.Abp.Identity.AbpIdentityApplicationModule模块
* 提供身份认证相关的应用服务实现
* 集成AutoMapper对象映射
* 支持用户头像URL扩展属性

## 模块引用

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

## 应用服务

* IdentityUserAppService - 用户管理服务
* IdentityRoleAppService - 角色管理服务
* IdentityClaimTypeAppService - 声明类型管理服务
* IdentitySecurityLogAppService - 安全日志服务
* IdentitySettingsAppService - 身份认证设置服务
* ProfileAppService - 用户配置文件服务

## 对象映射

模块使用AutoMapper进行对象映射，主要映射包括：

* IdentityUser -> IdentityUserDto
* IdentityRole -> IdentityRoleDto
* IdentityClaimType -> IdentityClaimTypeDto
* IdentitySecurityLog -> IdentitySecurityLogDto

## 基本用法

1. 使用用户管理服务
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

2. 使用角色管理服务
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

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
