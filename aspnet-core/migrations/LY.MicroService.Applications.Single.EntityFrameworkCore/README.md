# LY.MicroService.Applications.Single.EntityFrameworkCore

单体应用数据迁移模块，提供完整的应用程序数据库迁移功能。

[English](./README.EN.md)

## 功能特性

* 集成审计日志数据迁移
* 集成设置管理数据迁移
* 集成权限管理数据迁移
* 集成特性管理数据迁移
* 集成通知系统数据迁移
* 集成消息服务数据迁移
* 集成平台管理数据迁移
* 集成本地化管理数据迁移
* 集成身份认证数据迁移
* 集成IdentityServer数据迁移
* 集成OpenIddict数据迁移
* 集成文本模板数据迁移
* 集成Webhooks管理数据迁移
* 集成任务管理数据迁移
* 集成SaaS多租户数据迁移
* 支持数据库迁移事件处理
* 提供数据库迁移服务

## 模块依赖

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

## 配置项

```json
{
  "ConnectionStrings": {
    "SingleDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置SingleDbMigrator连接字符串

2. 添加模块依赖
   ```csharp
   [DependsOn(typeof(SingleMigrationsEntityFrameworkCoreModule))]
   public class YourModule : AbpModule
   {
       // ...
   }
   ```

## 数据库表说明

* AbpAuditLogs - 审计日志数据
* Identity相关表 - 用户、角色、声明等身份认证相关数据
* IdentityServer相关表 - 客户端、API资源、身份资源等OAuth/OpenID Connect相关数据
* OpenIddict相关表 - OpenID Connect认证相关数据
* AbpPermissionGrants - 权限授权数据
* AbpSettings - 系统设置数据
* AbpFeatures - 功能特性数据
* AbpNotifications - 通知系统数据
* AbpMessageService - 消息服务数据
* Platform相关表 - 平台管理相关数据
* AbpLocalization - 本地化管理数据
* AbpTextTemplates - 文本模板数据
* AbpWebhooks - Webhooks管理数据
* AbpTasks - 任务管理数据
* Saas相关表 - 租户、版本等多租户相关数据

## 更多信息

* [ABP文档](https://docs.abp.io)
* [OpenIddict文档](https://documentation.openiddict.com)
* [IdentityServer4文档](https://identityserver4.readthedocs.io)
