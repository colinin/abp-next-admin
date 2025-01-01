# LY.MicroService.IdentityServer.DbMigrator

身份认证服务器数据库迁移控制台应用程序，用于执行身份认证服务器的数据库迁移和初始化种子数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 初始化系统必要的种子数据
* 支持命令行参数配置
* 集成Autofac依赖注入容器
* 继承身份认证服务器的所有迁移功能
* 支持IdentityServer4配置
* 支持OAuth 2.0和OpenID Connect协议
* 支持用户和角色管理
* 支持客户端和API资源管理

## 模块依赖

```csharp
[DependsOn(
    typeof(IdentityServerMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## 配置项

```json
{
  "ConnectionStrings": {
    "IdentityServerDbMigrator": "你的数据库连接字符串"
  },
  "IdentityServer": {
    "Clients": {
      "IdentityServer_App": {
        "ClientId": "IdentityServer_App"
      }
    }
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置IdentityServerDbMigrator连接字符串

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

## 种子数据

* 默认用户和角色
* 标准身份资源
* API资源和范围
* 默认客户端配置
* 基本权限配置

## 更多信息

* [ABP文档](https://docs.abp.io)
* [IdentityServer4文档](https://identityserver4.readthedocs.io)
* [OAuth 2.0规范](https://oauth.net/2/)
* [OpenID Connect规范](https://openid.net/connect/)
