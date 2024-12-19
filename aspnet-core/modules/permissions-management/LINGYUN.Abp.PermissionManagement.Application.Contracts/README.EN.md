# LINGYUN.Abp.PermissionManagement.Application.Contracts

Permission management application service contract module, defining interfaces and DTOs for permission management.

## Features

* Permission Group Definition Interfaces
  * Provides CRUD operation interfaces for permission groups
  * Supports pagination query for permission groups
* Permission Definition Interfaces
  * Provides CRUD operation interfaces for permissions
  * Supports pagination query for permissions
* Permission Definition DTOs
  * PermissionGroupDefinitionDto - Permission group definition DTO
  * PermissionDefinitionDto - Permission definition DTO
  * Supports permission provider configuration (Role, User, OrganizationUnit, etc.)
* Permission Error Codes
  * 001100 - Permission group already exists
  * 001010 - Static permission group is not allowed to change
  * 001404 - Permission group not found
  * 002100 - Permission already exists
  * 002010 - Static permission is not allowed to change
  * 002101 - Could not retrieve the group definition of permission
  * 002404 - Permission not found

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Permission Definition

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

## See Also

* [LINGYUN.Abp.PermissionManagement.Application](../LINGYUN.Abp.PermissionManagement.Application/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits](../LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.HttpApi](../LINGYUN.Abp.PermissionManagement.HttpApi/README.EN.md)
