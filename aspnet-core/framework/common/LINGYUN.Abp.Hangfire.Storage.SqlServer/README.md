# LINGYUN.Abp.Hangfire.Storage.SqlServer

[English](README.EN.md) | 简体中文

## 1. 介绍

`LINGYUN.Abp.Hangfire.Storage.SqlServer` 是一个用于配置Hangfire使用SQL Server作为存储的ABP模块。该模块提供了简单的配置方式，让你能够轻松地将SQL Server作为Hangfire的持久化存储。

## 2. 功能特性

* 简单的SQL Server存储配置
* 支持自定义连接字符串
* 支持SQL Server存储选项配置
* 与ABP配置系统无缝集成

## 3. 安装

```bash
dotnet add package LINGYUN.Abp.Hangfire.Storage.SqlServer
```

## 4. 使用方法

1. 添加 `AbpHangfireSqlServerStorageModule` 到模块依赖中:

```csharp
[DependsOn(typeof(AbpHangfireSqlServerStorageModule))]
public class YourModule : AbpModule
{
}
```

2. 在appsettings.json中配置连接字符串和存储选项:

```json
{
  "Hangfire": {
    "SqlServer": {
      "Connection": "Server=localhost;Database=YourDb;Trusted_Connection=True;",
      "CommandBatchMaxTimeout": "00:05:00",
      "SlidingInvisibilityTimeout": "00:05:00",
      "QueuePollInterval": "00:00:00",
      "UseRecommendedIsolationLevel": true,
      "DisableGlobalLocks": true
    }
  }
}
```

## 5. 配置项

### 5.1 连接字符串配置

模块会按以下顺序查找连接字符串：
1. `Hangfire:SqlServer:Connection`
2. `ConnectionStrings:Default`

### 5.2 存储选项

* `CommandBatchMaxTimeout`: 命令批处理最大超时时间
* `SlidingInvisibilityTimeout`: 滑动不可见超时时间
* `QueuePollInterval`: 队列轮询间隔
* `UseRecommendedIsolationLevel`: 是否使用推荐的隔离级别
* `DisableGlobalLocks`: 是否禁用全局锁

## 6. 依赖项

* Volo.Abp.Hangfire
* Hangfire.SqlServer

## 7. 文档和资源

* [Hangfire官方文档](https://docs.hangfire.io/)
* [Hangfire.SqlServer文档](https://docs.hangfire.io/en/latest/configuration/using-sql-server.html)
* [ABP框架文档](https://docs.abp.io/)
