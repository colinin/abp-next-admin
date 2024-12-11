# LY.MicroService.AuthServer.DbMigrator

认证服务器数据库迁移控制台应用程序，用于执行认证服务器的数据库迁移和初始化种子数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 初始化系统必要的种子数据
* 支持命令行参数配置
* 集成Autofac依赖注入容器
* 继承认证服务器的所有迁移功能

## 模块依赖

```csharp
[DependsOn(
    typeof(AuthServerMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## 配置项

```json
{
  "ConnectionStrings": {
    "AuthServerDbMigrator": "你的数据库连接字符串"
  },
  "IdentityServer": {
    "Clients": {
      "AuthServer_App": {
        "ClientId": "AuthServer_App"
      }
    }
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置AuthServerDbMigrator连接字符串

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
