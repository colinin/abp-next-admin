# LY.MicroService.LocalizationManagement.EntityFrameworkCore

Localization Management Database Migration Module, providing database migration functionality for the localization management system.

[简体中文](./README.md)

## Features

* Integrated SaaS multi-tenancy data migration
* Integrated Localization Management data migration
* Integrated Setting Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "LocalizationManagementDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure LocalizationManagementDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(LocalizationManagementMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* AbpLocalization - Localization resources and text data
* Saas Related Tables - Tenant, edition and other multi-tenancy related data
* AbpSettings - System settings data
* AbpPermissionGrants - Permission authorization data
* AbpFeatures - Feature data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ABP Localization Documentation](https://docs.abp.io/en/abp/latest/Localization)
