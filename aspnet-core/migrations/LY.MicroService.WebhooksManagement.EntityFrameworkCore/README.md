# LY.MicroService.WebhooksManagement.EntityFrameworkCore

Webhooks管理数据迁移模块，提供Webhooks管理系统相关的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成SaaS多租户数据迁移
* 集成Webhooks管理数据迁移
* 集成设置管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 支持MySQL数据库

## 模块依赖

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

## 配置项

```json
{
  "ConnectionStrings": {
    "WebhooksManagementDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置WebhooksManagementDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(WebhooksManagementMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* AbpWebhooks - Webhooks基本信息表
* AbpWebhookSubscriptions - Webhooks订阅表
* AbpWebhookGroups - Webhooks分组表
* AbpWebhookEvents - Webhooks事件表
* AbpWebhookSendAttempts - Webhooks发送记录表
* Saas相关表 - 租户、版本等多租户相关数据
* AbpSettings - 系统设置数据
* AbpPermissionGrants - 权限授权数据
* AbpFeatures - 功能特性数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ABP Webhooks模块](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/modules/webhooks-management)
