# LINGYUN.Abp.Identity.Application.Contracts

身份认证应用服务契约模块，提供身份认证相关的应用服务接口定义。

## 功能特性

* 扩展Volo.Abp.Identity.AbpIdentityApplicationContractsModule模块
* 提供身份认证相关的应用服务接口定义
* 提供身份认证相关的DTO对象定义
* 集成ABP授权模块

## 模块引用

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

## 服务接口

* IIdentityUserAppService - 用户管理服务接口
* IIdentityRoleAppService - 角色管理服务接口
* IIdentityClaimTypeAppService - 声明类型管理服务接口
* IIdentitySecurityLogAppService - 安全日志服务接口
* IIdentitySettingsAppService - 身份认证设置服务接口
* IProfileAppService - 用户配置文件服务接口

## DTO对象

* IdentityUserDto - 用户DTO
* IdentityRoleDto - 角色DTO
* IdentityClaimTypeDto - 声明类型DTO
* IdentitySecurityLogDto - 安全日志DTO
* GetIdentityUsersInput - 获取用户列表输入DTO
* GetIdentityRolesInput - 获取角色列表输入DTO
* IdentityUserCreateDto - 创建用户DTO
* IdentityUserUpdateDto - 更新用户DTO
* IdentityRoleCreateDto - 创建角色DTO
* IdentityRoleUpdateDto - 更新角色DTO

## 基本用法

1. 实现用户管理服务接口
```csharp
public class YourIdentityUserAppService : IIdentityUserAppService
{
    public async Task<IdentityUserDto> GetAsync(Guid id)
    {
        // 实现获取用户的逻辑
    }

    public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        // 实现获取用户列表的逻辑
    }
}
```

2. 实现角色管理服务接口
```csharp
public class YourIdentityRoleAppService : IIdentityRoleAppService
{
    public async Task<IdentityRoleDto> GetAsync(Guid id)
    {
        // 实现获取角色的逻辑
    }

    public async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        // 实现获取角色列表的逻辑
    }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
