# LY.MicroService.RealtimeMessage.DbMigrator

实时消息数据库迁移控制台应用程序，用于执行实时消息系统的数据库迁移和初始化种子数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 初始化系统必要的种子数据
* 支持命令行参数配置
* 集成Autofac依赖注入容器
* 继承实时消息系统的所有迁移功能
* 支持SignalR消息管理
* 支持消息分组管理
* 支持消息订阅管理
* 支持消息历史记录

## 模块依赖

```csharp
[DependsOn(
    typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## 配置项

```json
{
  "ConnectionStrings": {
    "RealtimeMessageDbMigrator": "你的数据库连接字符串"
  }
}
```

## 基本用法

1. 配置数据库连接字符串
   * 在appsettings.json中配置RealtimeMessageDbMigrator连接字符串

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

* 默认消息组配置
* 基础消息订阅配置
* 系统消息模板
* 默认权限配置

## 更多信息

* [ABP文档](https://docs.abp.io)
* [SignalR文档](https://docs.microsoft.com/aspnet/core/signalr/introduction)
