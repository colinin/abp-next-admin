# LY.MicroService.BackendAdmin.EntityFrameworkCore

后台管理数据迁移模块，提供后台管理系统相关的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成SaaS多租户数据迁移
* 集成设置管理数据迁移
* 集成数据保护管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 集成文本模板数据迁移
* 支持MySQL数据库

## 模块依赖

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

## 配置项

```json
{
  "ConnectionStrings": {
    "BackendAdminDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置BackendAdminDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(BackendAdminMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* Saas相关表 - 租户、版本等多租户相关数据
* AbpSettings - 系统设置数据
* AbpDataProtection - 数据保护相关数据
* AbpPermissionGrants - 权限授权数据
* AbpFeatures - 功能特性数据
* AbpTextTemplates - 文本模板数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ASP.NET Core数据保护](https://docs.microsoft.com/zh-cn/aspnet/core/security/data-protection/introduction)
