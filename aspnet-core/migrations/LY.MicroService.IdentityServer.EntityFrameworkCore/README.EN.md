# LY.MicroService.IdentityServer.EntityFrameworkCore

Identity Server Database Migration Module, providing database migration functionality for IdentityServer.

[简体中文](./README.md)

## Features

* Integrated ABP Identity data migration
* Integrated IdentityServer data migration
* Integrated Permission Management data migration
* Integrated Setting Management data migration
* Integrated Feature Management data migration
* Integrated Text Templating data migration
* Integrated SaaS multi-tenancy data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpWeChatModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "IdentityServerDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure IdentityServerDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(IdentityServerMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* Identity Related Tables - User, role, claims and other authentication related data
* IdentityServer Related Tables - Client, API resources, identity resources and other OAuth/OpenID Connect related data
* AbpPermissionGrants - Permission authorization data
* AbpSettings - System settings data
* AbpFeatures - Feature data
* AbpTextTemplates - Text template data
* Saas Related Tables - Tenant, edition and other multi-tenancy related data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io)
