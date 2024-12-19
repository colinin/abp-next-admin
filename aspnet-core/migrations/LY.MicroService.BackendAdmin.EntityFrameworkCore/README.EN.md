# LY.MicroService.BackendAdmin.EntityFrameworkCore

Backend Administration Database Migration Module, providing database migration functionality for the backend management system.

[简体中文](./README.md)

## Features

* Integrated SaaS multi-tenancy data migration
* Integrated Setting Management data migration
* Integrated Data Protection Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Integrated Text Templating data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "BackendAdminDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure BackendAdminDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(BackendAdminMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* Saas Related Tables - Tenant, edition and other multi-tenancy related data
* AbpSettings - System settings data
* AbpDataProtection - Data protection related data
* AbpPermissionGrants - Permission authorization data
* AbpFeatures - Feature data
* AbpTextTemplates - Text template data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ASP.NET Core Data Protection](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/introduction)
