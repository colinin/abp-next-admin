# LY.MicroService.TaskManagement.EntityFrameworkCore

任务管理数据迁移模块，提供任务管理系统相关的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成SaaS多租户数据迁移
* 集成任务管理数据迁移
* 集成设置管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 支持MySQL数据库

## 模块依赖

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

## 配置项

```json
{
  "ConnectionStrings": {
    "TaskManagementDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置TaskManagementDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(TaskManagementMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* AbpTasks - 任务基本信息表
* AbpTaskCategories - 任务分类表
* AbpTaskStatuses - 任务状态表
* AbpTaskAssignments - 任务分配表
* Saas相关表 - 租户、版本等多租户相关数据
* AbpSettings - 系统设置数据
* AbpPermissionGrants - 权限授权数据
* AbpFeatures - 功能特性数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ABP任务管理模块](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/modules/task-management)
