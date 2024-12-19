# LY.MicroService.Platform.EntityFrameworkCore

平台管理数据迁移模块，提供平台管理系统相关的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成SaaS多租户数据迁移
* 集成平台管理数据迁移
* 集成设置管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 集成Vue Vben Admin UI导航数据迁移
* 支持MySQL数据库

## 模块依赖

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpUINavigationVueVbenAdminModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## 配置项

```json
{
  "ConnectionStrings": {
    "PlatformDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置PlatformDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(PlatformMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* Platform相关表 - 包含数据字典、组织机构、菜单、版本等平台基础数据
* Saas相关表 - 租户、版本等多租户相关数据
* AbpSettings - 系统设置数据
* AbpPermissionGrants - 权限授权数据
* AbpFeatures - 功能特性数据
* AbpUINavigation - Vue Vben Admin UI导航数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [Vue Vben Admin文档](https://doc.vvbin.cn/)
