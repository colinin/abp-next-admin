# LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits

Organization unit permission management domain module, providing permission management functionality based on organization units.

## Features

* Organization Unit Permission Management
  * Support assigning permissions to organization units
  * Support organization unit permission inheritance
* Permission Provider
  * Implement OrganizationUnit permission provider
  * Support role organization unit permission check
  * Support user organization unit permission check
* Automatic Permission Cleanup
  * Automatically clean up related permissions when organization unit is deleted

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpPermissionManagementDomainOrganizationUnitsModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Basic Usage

1. Organization Unit Permission Management
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
        // Set organization unit permission
        await _permissionManager.SetAsync(
            "MyPermission",
            OrganizationUnitPermissionValueProvider.ProviderName,
            organizationUnitCode);

        // Check organization unit permission
        var result = await _permissionManager.GetAsync(
            "MyPermission",
            OrganizationUnitPermissionValueProvider.ProviderName,
            organizationUnitCode);
    }
}
```

## See Also

* [LINGYUN.Abp.PermissionManagement.Application](../LINGYUN.Abp.PermissionManagement.Application/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.Application.Contracts](../LINGYUN.Abp.PermissionManagement.Application.Contracts/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.HttpApi](../LINGYUN.Abp.PermissionManagement.HttpApi/README.EN.md)
