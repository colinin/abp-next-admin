# LY.MicroService.RealtimeMessage.EntityFrameworkCore

Real-time Message Database Migration Module, providing database migration functionality for real-time messaging and notification systems.

[简体中文](./README.md)

## Features

* Integrated SaaS multi-tenancy data migration
* Integrated Notification System data migration
* Integrated Message Service data migration
* Integrated Text Templating data migration
* Integrated Setting Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
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
    "RealtimeMessageDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure RealtimeMessageDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* AbpNotifications - Notification system data tables
* AbpNotificationsDefinition - Notification definition data tables
* AbpMessageService - Message service data tables
* Saas Related Tables - Tenant, edition and other multi-tenancy related data
* AbpSettings - System settings data
* AbpPermissionGrants - Permission authorization data
* AbpFeatures - Feature data
* AbpTextTemplates - Text template data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ABP Notification System Documentation](https://docs.abp.io/en/abp/latest/Notification-System)
