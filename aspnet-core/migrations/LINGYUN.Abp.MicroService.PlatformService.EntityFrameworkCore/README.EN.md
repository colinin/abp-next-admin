# LY.MicroService.Platform.EntityFrameworkCore

Platform Management Database Migration Module, providing database migration functionality for the platform management system.

[简体中文](./README.md)

## Features

* Integrated SaaS multi-tenancy data migration
* Integrated Platform Management data migration
* Integrated Setting Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Integrated Vue Vben Admin UI navigation data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpUINavigationVueVbenAdminModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "PlatformDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure PlatformDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(PlatformMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* Platform Related Tables - Contains data dictionary, organization structure, menu, version and other platform basic data
* Saas Related Tables - Tenant, edition and other multi-tenancy related data
* AbpSettings - System settings data
* AbpPermissionGrants - Permission authorization data
* AbpFeatures - Feature data
* AbpUINavigation - Vue Vben Admin UI navigation data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [Vue Vben Admin Documentation](https://doc.vvbin.cn/)
