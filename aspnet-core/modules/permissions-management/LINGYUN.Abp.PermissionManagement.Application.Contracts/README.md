# LINGYUN.Abp.PermissionManagement.Application.Contracts

权限管理应用服务契约模块，定义了权限管理的接口和DTO。

## 功能特性

* 权限组定义接口
  * 提供权限组的CRUD操作接口
  * 支持权限组分页查询
* 权限定义接口
  * 提供权限的CRUD操作接口
  * 支持权限分页查询
* 权限定义DTO
  * PermissionGroupDefinitionDto - 权限组定义DTO
  * PermissionDefinitionDto - 权限定义DTO
  * 支持权限提供者配置（Role、User、OrganizationUnit等）
* 权限错误代码定义
  * 001100 - 权限组已存在
  * 001010 - 静态权限组不允许修改
  * 001404 - 权限组不存在
  * 002100 - 权限已存在
  * 002010 - 静态权限不允许修改
  * 002101 - 无法获取权限的组定义
  * 002404 - 权限不存在

## 模块引用

```csharp
[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 权限定义

```csharp
public class YourPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var permissionGroup = context.AddGroup(
            "YourPermissionGroup",
            "Your Permission Group");

        var permission = permissionGroup.AddPermission(
            "YourPermission",
            "Your Permission",
            MultiTenancySides.Both);

        permission.AddChild(
            "Create",
            "Create Permission");
    }
}
```

## 另请参阅

* [LINGYUN.Abp.PermissionManagement.Application](../LINGYUN.Abp.PermissionManagement.Application/README.md)
* [LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits](../LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits/README.md)
* [LINGYUN.Abp.PermissionManagement.HttpApi](../LINGYUN.Abp.PermissionManagement.HttpApi/README.md)
