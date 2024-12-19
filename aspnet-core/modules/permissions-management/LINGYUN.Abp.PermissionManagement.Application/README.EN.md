# LINGYUN.Abp.PermissionManagement.Application

Permission management application service module, providing application layer implementation for permission management.

## Features

* Permission Group Definition Management
  * Create, update, and delete permission group definitions
  * Support enabling/disabling permission groups
  * Support static/dynamic configuration of permission groups
* Permission Definition Management
  * Create, update, and delete permission definitions
  * Support enabling/disabling permissions
  * Support static/dynamic configuration of permissions
  * Support parent-child hierarchy relationships for permissions
* Multi-tenancy Support
  * Support both Host and Tenant multi-tenancy modes
  * Support multi-tenancy side configuration for permissions

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(VoloAbpPermissionManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Basic Usage

1. Permission Group Definition Management
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
        // Create permission group
        await _groupDefinitionAppService.CreateAsync(new PermissionGroupDefinitionCreateDto
        {
            Name = "MyPermissionGroup",
            DisplayName = "My Permission Group"
        });

        // Update permission group
        await _groupDefinitionAppService.UpdateAsync("MyPermissionGroup", 
            new PermissionGroupDefinitionUpdateDto
            {
                DisplayName = "Updated Permission Group"
            });
    }
}
```

2. Permission Definition Management
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
        // Create permission
        await _permissionDefinitionAppService.CreateAsync(new PermissionDefinitionCreateDto
        {
            GroupName = "MyPermissionGroup",
            Name = "MyPermission",
            DisplayName = "My Permission",
            Providers = new[] { "Role", "User" }
        });

        // Update permission
        await _permissionDefinitionAppService.UpdateAsync("MyPermission",
            new PermissionDefinitionUpdateDto
            {
                DisplayName = "Updated Permission"
            });
    }
}
```

## See Also

* [LINGYUN.Abp.PermissionManagement.Application.Contracts](../LINGYUN.Abp.PermissionManagement.Application.Contracts/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits](../LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.HttpApi](../LINGYUN.Abp.PermissionManagement.HttpApi/README.EN.md)
