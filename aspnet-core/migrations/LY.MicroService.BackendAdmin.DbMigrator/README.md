# LY.MicroService.BackendAdmin.DbMigrator

后台管理系统数据库迁移控制台应用程序，用于执行后台管理系统的数据库迁移和初始化种子数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 初始化系统必要的种子数据
* 支持命令行参数配置
* 集成Autofac依赖注入容器
* 集成特性管理功能
* 集成设置管理功能
* 集成权限管理功能
* 集成本地化管理功能
* 集成缓存管理功能
* 集成审计日志功能
* 集成文本模板功能
* 集成身份认证功能
* 集成IdentityServer功能
* 集成OpenIddict功能
* 集成平台管理功能
* 集成对象存储功能
* 集成通知系统功能
* 集成消息服务功能
* 集成任务管理功能
* 集成Webhooks管理功能

## 模块依赖

```csharp
[DependsOn(
    typeof(BackendAdminMigrationsEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpLocalizationManagementApplicationContractsModule),
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpAuditingApplicationContractsModule),
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpIdentityServerApplicationContractsModule),
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(PlatformApplicationContractModule),
    typeof(AbpOssManagementApplicationContractsModule),
    typeof(AbpNotificationsApplicationContractsModule),
    typeof(AbpMessageServiceApplicationContractsModule),
    typeof(TaskManagementApplicationContractsModule),
    typeof(WebhooksManagementApplicationContractsModule),
    typeof(AbpAutofacModule)
)]
```

## 配置项

```json
{
  "ConnectionStrings": {
    "BackendAdminDbMigrator": "你的数据库连接字符串"
  },
  "IdentityServer": {
    "Clients": {
      "BackendAdmin_App": {
        "ClientId": "BackendAdmin_App"
      }
    }
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置BackendAdminDbMigrator连接字符串

2. 运行迁移程序
   ```bash
   dotnet run
   ```

## 命令行参数

* --database-provider
  * 指定数据库提供程序 (默认: MySQL)
* --connection-string
  * 指定数据库连接字符串
* --skip-db-migrations
  * 跳过数据库迁移
* --skip-seed-data
  * 跳过种子数据初始化

## 更多信息

* [ABP文档](https://docs.abp.io)
* [EF Core迁移文档](https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/)
* [IdentityServer4文档](https://identityserver4.readthedocs.io)
* [OpenIddict文档](https://documentation.openiddict.com)
