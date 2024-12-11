# LY.MicroService.IdentityServer.EntityFrameworkCore

身份认证服务器数据迁移模块，提供IdentityServer的数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成ABP Identity数据迁移
* 集成IdentityServer数据迁移
* 集成权限管理数据迁移
* 集成设置管理数据迁移
* 集成特性管理数据迁移
* 集成文本模板数据迁移
* 集成SaaS多租户数据迁移
* 支持MySQL数据库

## 模块依赖

```csharp
[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpWeChatModule),
    typeof(AbpDataDbMigratorModule)
)]
```

## 配置项

```json
{
  "ConnectionStrings": {
    "IdentityServerDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置IdentityServerDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(IdentityServerMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* Identity相关表 - 用户、角色、声明等身份认证相关数据
* IdentityServer相关表 - 客户端、API资源、身份资源等OAuth/OpenID Connect相关数据
* AbpPermissionGrants - 权限授权数据
* AbpSettings - 系统设置数据
* AbpFeatures - 功能特性数据
* AbpTextTemplates - 文本模板数据
* Saas相关表 - 租户、版本等多租户相关数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [IdentityServer4文档](https://identityserver4.readthedocs.io)
