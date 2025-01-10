# LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits

组织单元权限管理领域模块，提供基于组织单元的权限管理功能。

## 功能特性

* 组织单元权限管理
  * 支持为组织单元分配权限
  * 支持组织单元权限的继承
* 权限提供者
  * 实现OrganizationUnit权限提供者
  * 支持角色组织单元权限检查
  * 支持用户组织单元权限检查
* 自动权限清理
  * 组织单元删除时自动清理相关权限

## 模块引用

```csharp
[DependsOn(
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 基本用法

1. 组织单元权限管理
```csharp
public class YourService
{
    private readonly IPermissionManager _permissionManager;

    public YourService(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    public async Task ManageOrganizationUnitPermissionAsync(string organizationUnitCode)
    {
        // 设置组织单元权限
        await _permissionManager.SetAsync(
            "MyPermission",
            OrganizationUnitPermissionValueProvider.ProviderName,
            organizationUnitCode);

        // 检查组织单元权限
        var result = await _permissionManager.GetAsync(
            "MyPermission",
            OrganizationUnitPermissionValueProvider.ProviderName,
            organizationUnitCode);
    }
}
```

## 另请参阅

* [LINGYUN.Abp.PermissionManagement.Application](../LINGYUN.Abp.PermissionManagement.Application/README.md)
* [LINGYUN.Abp.PermissionManagement.Application.Contracts](../LINGYUN.Abp.PermissionManagement.Application.Contracts/README.md)
* [LINGYUN.Abp.PermissionManagement.HttpApi](../LINGYUN.Abp.PermissionManagement.HttpApi/README.md)
