# LY.MicroService.LocalizationManagement.DbMigrator

本地化管理数据库迁移控制台应用程序，用于执行本地化管理系统的数据库迁移和初始化种子数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 初始化系统必要的种子数据
* 支持命令行参数配置
* 集成Autofac依赖注入容器
* 继承本地化管理系统的所有迁移功能
* 支持多语言资源管理
* 支持动态语言文本管理
* 支持语言管理

## 模块依赖

```csharp
[DependsOn(
    typeof(LocalizationManagementMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
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

* 默认语言配置
* 基础本地化资源
* 系统文本资源
* 默认权限配置

## 更多信息

* [ABP文档](https://docs.abp.io)
* [ABP本地化文档](https://docs.abp.io/zh-Hans/abp/latest/Localization)
