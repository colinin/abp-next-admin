# LY.MicroService.Applications.Single.EntityFrameworkCore

Monolithic Application Database Migration Module, providing comprehensive application database migration functionality.

[简体中文](./README.md)

## Features

* Integrated Audit Logging data migration
* Integrated Setting Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Integrated Notification System data migration
* Integrated Message Service data migration
* Integrated Platform Management data migration
* Integrated Localization Management data migration
* Integrated Identity Authentication data migration
* Integrated IdentityServer data migration
* Integrated OpenIddict data migration
* Integrated Text Templating data migration
* Integrated Webhooks Management data migration
* Integrated Task Management data migration
* Integrated SaaS multi-tenancy data migration
* Support for database migration event handling
* Provides database migration service

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpLocalizationManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(WebhooksManagementEntityFrameworkCoreModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpWeChatModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "SingleDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure SingleDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(SingleMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* AbpAuditLogs - Audit logging data
* Identity Related Tables - User, role, claims and other authentication related data
* IdentityServer Related Tables - Client, API resources, identity resources and other OAuth/OpenID Connect related data
* OpenIddict Related Tables - OpenID Connect authentication related data
* AbpPermissionGrants - Permission authorization data
* AbpSettings - System settings data
* AbpFeatures - Feature data
* AbpNotifications - Notification system data
* AbpMessageService - Message service data
* Platform Related Tables - Platform management related data
* AbpLocalization - Localization management data
* AbpTextTemplates - Text template data
* AbpWebhooks - Webhooks management data
* AbpTasks - Task management data
* Saas Related Tables - Tenant, edition and other multi-tenancy related data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [OpenIddict Documentation](https://documentation.openiddict.com)
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io)
