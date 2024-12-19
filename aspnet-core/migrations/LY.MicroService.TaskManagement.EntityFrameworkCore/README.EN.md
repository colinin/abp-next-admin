# LY.MicroService.TaskManagement.EntityFrameworkCore

Task Management Database Migration Module, providing database migration functionality for the task management system.

[简体中文](./README.md)

## Features

* Integrated SaaS multi-tenancy data migration
* Integrated Task Management data migration
* Integrated Setting Management data migration
* Integrated Permission Management data migration
* Integrated Feature Management data migration
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
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
    "TaskManagementDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure TaskManagementDbMigrator connection string in appsettings.json

2. Add Module Dependency
   ```csharp
   [DependsOn(typeof(TaskManagementMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## Database Tables Description

* AbpTasks - Task basic information table
* AbpTaskCategories - Task categories table
* AbpTaskStatuses - Task statuses table
* AbpTaskAssignments - Task assignments table
* Saas Related Tables - Tenant, edition and other multi-tenancy related data
* AbpSettings - System settings data
* AbpPermissionGrants - Permission authorization data
* AbpFeatures - Feature data

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ABP Task Management Module](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/modules/task-management)
