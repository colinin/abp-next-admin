# LY.MicroService.Platform.DbMigrator

平台管理数据库迁移控制台应用程序，用于执行平台管理系统的数据库迁移和初始化种子数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 初始化系统必要的种子数据
* 支持命令行参数配置
* 集成Autofac依赖注入容器
* 继承平台管理系统的所有迁移功能
* 支持数据字典管理
* 支持组织机构管理
* 支持菜单管理
* 支持版本管理
* 支持Vue Vben Admin UI导航

## 模块依赖

```csharp
[DependsOn(
    typeof(PlatformMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
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

* 默认数据字典
* 基础组织机构
* 系统菜单
* 默认版本配置
* UI导航配置

## 更多信息

* [ABP文档](https://docs.abp.io)
* [Vue Vben Admin文档](https://doc.vvbin.cn/)
