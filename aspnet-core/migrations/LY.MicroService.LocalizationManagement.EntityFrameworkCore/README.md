# LY.MicroService.LocalizationManagement.EntityFrameworkCore

本地化管理数据迁移模块，提供本地化管理系统相关的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成SaaS多租户数据迁移
* 集成本地化管理数据迁移
* 集成设置管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 支持MySQL数据库

## 模块依赖

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

## 配置项

```json
{
  "ConnectionStrings": {
    "LocalizationManagementDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置LocalizationManagementDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(LocalizationManagementMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* AbpLocalization - 本地化资源和文本数据
* Saas相关表 - 租户、版本等多租户相关数据
* AbpSettings - 系统设置数据
* AbpPermissionGrants - 权限授权数据
* AbpFeatures - 功能特性数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ABP本地化文档](https://docs.abp.io/zh-Hans/abp/latest/Localization)
