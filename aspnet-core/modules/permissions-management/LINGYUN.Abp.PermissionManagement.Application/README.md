# LINGYUN.Abp.PermissionManagement.Application

权限管理应用服务模块，提供权限管理的应用层实现。

## 功能特性

* 权限组定义管理
  * 创建、更新、删除权限组定义
  * 支持权限组的启用/禁用
  * 支持权限组的静态/动态配置
* 权限定义管理
  * 创建、更新、删除权限定义
  * 支持权限的启用/禁用
  * 支持权限的静态/动态配置
  * 支持权限的父子层级关系
* 多租户支持
  * 支持Host和Tenant两种多租户模式
  * 支持权限的多租户侧配置

## 模块引用

```csharp
[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(VoloAbpPermissionManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 基本用法

1. 权限组定义管理
```csharp
public class YourService
{
    private readonly IPermissionGroupDefinitionAppService _groupDefinitionAppService;

    public YourService(IPermissionGroupDefinitionAppService groupDefinitionAppService)
    {
        _groupDefinitionAppService = groupDefinitionAppService;
    }

    public async Task ManageGroupDefinitionAsync()
    {
        // 创建权限组
        await _groupDefinitionAppService.CreateAsync(new PermissionGroupDefinitionCreateDto
        {
            Name = "MyPermissionGroup",
            DisplayName = "My Permission Group"
        });

        // 更新权限组
        await _groupDefinitionAppService.UpdateAsync("MyPermissionGroup", 
            new PermissionGroupDefinitionUpdateDto
            {
                DisplayName = "Updated Permission Group"
            });
    }
}
```

2. 权限定义管理
```csharp
public class YourService
{
    private readonly IPermissionDefinitionAppService _permissionDefinitionAppService;

    public YourService(IPermissionDefinitionAppService permissionDefinitionAppService)
    {
        _permissionDefinitionAppService = permissionDefinitionAppService;
    }

    public async Task ManagePermissionDefinitionAsync()
    {
        // 创建权限
        await _permissionDefinitionAppService.CreateAsync(new PermissionDefinitionCreateDto
        {
            GroupName = "MyPermissionGroup",
            Name = "MyPermission",
            DisplayName = "My Permission",
            Providers = new[] { "Role", "User" }
        });

        // 更新权限
        await _permissionDefinitionAppService.UpdateAsync("MyPermission",
            new PermissionDefinitionUpdateDto
            {
                DisplayName = "Updated Permission"
            });
    }
}
```

## 另请参阅

* [LINGYUN.Abp.PermissionManagement.Application.Contracts](../LINGYUN.Abp.PermissionManagement.Application.Contracts/README.md)
* [LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits](../LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits/README.md)
* [LINGYUN.Abp.PermissionManagement.HttpApi](../LINGYUN.Abp.PermissionManagement.HttpApi/README.md)
