# LY.MicroService.WebhooksManagement.EntityFrameworkCore

Webhooks Management Database Migration Module, providing database migration functionality for the webhooks management system.

[简体中文](./README.md)

## Features

* Integrated SaaS multi-tenancy data migration
* Integrated Webhooks Management data migration
* Integrated Setting Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(WebhooksManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
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
    "WebhooksManagementDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure WebhooksManagementDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(WebhooksManagementMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* AbpWebhooks - Webhooks basic information table
* AbpWebhookSubscriptions - Webhooks subscriptions table
* AbpWebhookGroups - Webhooks groups table
* AbpWebhookEvents - Webhooks events table
* AbpWebhookSendAttempts - Webhooks send attempts table
* Saas Related Tables - Tenant, edition and other multi-tenancy related data
* AbpSettings - System settings data
* AbpPermissionGrants - Permission authorization data
* AbpFeatures - Feature data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ABP Webhooks Module](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/modules/webhooks-management)
