# LY.MicroService.RealtimeMessage.EntityFrameworkCore

实时消息数据迁移模块，提供实时消息和通知系统相关的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成SaaS多租户数据迁移
* 集成通知系统数据迁移
* 集成消息服务数据迁移
* 集成文本模板数据迁移
* 集成设置管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 支持MySQL数据库

## 模块依赖

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

## 配置项

```json
{
  "ConnectionStrings": {
    "RealtimeMessageDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置RealtimeMessageDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* AbpNotifications - 通知系统数据表
* AbpNotificationsDefinition - 通知定义数据表
* AbpMessageService - 消息服务数据表
* Saas相关表 - 租户、版本等多租户相关数据
* AbpSettings - 系统设置数据
* AbpPermissionGrants - 权限授权数据
* AbpFeatures - 功能特性数据
* AbpTextTemplates - 文本模板数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ABP通知系统文档](https://docs.abp.io/zh-Hans/abp/latest/Notification-System)
