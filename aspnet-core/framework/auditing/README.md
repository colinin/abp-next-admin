# LINGYUN.Abp 审计模块

## 模块概述

审计模块提供了全面的日志记录和审计功能，支持多种存储方式和高度可配置的审计选项。

## 功能特性

### 核心功能

- 审计日志基础设施
- 审计日志仓储接口定义
- 审计日志管理器接口定义
- 支持忽略特定类型的审计日志记录

### 存储支持

- EntityFrameworkCore 实现  
- Elasticsearch 实现  

> 注意: Elastic库限制, 兼容8.x; 9.x版本, 如需使用10.x版本, 请切换 **Elastic.Clients.Elasticsearch** 为9.x版本 
> 参考: https://www.nuget.org/packages/Elastic.Clients.Elasticsearch#readme-body-tab  

## 模块引用

### 核心模块

```csharp
[DependsOn(typeof(AbpAuditLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

### EntityFrameworkCore 模块

```csharp
[DependsOn(typeof(AbpAuditLoggingEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

### Elasticsearch 模块

```csharp
[DependsOn(typeof(AbpAuditLoggingElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置选项

### 审计日志配置

```json
{
  "Auditing": {
    "IsEnabled": true, // 是否启用审计日志
    "HideErrors": true, // 是否隐藏错误信息
    "IsEnabledForAnonymousUsers": true, // 是否为匿名用户启用审计日志
    "IsEnabledForGetRequests": false, // 是否为GET请求启用审计日志
    "ApplicationName": null // 应用程序名称
  },
  // 审计日志增强配置
  "AuditLogging": {
    "IsAuditLogEnabled": true, // 是否启用审计日志记录
    "UseAuditLogQueue": true, // 使用审计日志队列, 不启用队列则直接写入到持久化设施
    "MaxAuditLogQueueSize": 10000, // 审计日志最大队列大小, 默认10000
    "BatchAuditLogSize": 100, // 一次处理审计日志队列大小, 队列中同时写入100条记录后立即写入到持久化设施中
    "IsSecurityLogEnabled": true, // 是否启用安全日志记录
    "UseSecurityLogQueue": true, // 使用安全日志队列, 不启用队列则直接写入到持久化设施
    "MaxSecurityLogQueueSize": 10000, // 安全日志最大队列大小, 默认10000
    "BatchSecurityLogSize": 100 // 一次处理安全日志队列大小, 队列中同时写入100条记录后立即写入到持久化设施中
  }
}
```

### Elasticsearch 配置

```json
{
  "AuditLogging": {
    "Elasticsearch": {
      "IndexPrefix": "auditlogging", // 索引前缀
      "ThrowIfIndexInitFailed": true // 索引初始化失败抛出异常, 默认为: true, 索引初始化失败后应用程序停止运行
    }
  }
}
```

## 数据库连接配置

```json
{
  "ConnectionStrings": {
    "AbpIdentity": "Server=127.0.0.1;Database=Identity;User Id=root;Password=*",
    "AbpAuditLogging": "Server=127.0.0.1;Database=AuditLogging;User Id=root;Password=*"
  }
}
```

## 高级功能

### 忽略特定类型

默认情况下，模块会忽略以下类型的审计日志：

- CancellationToken
- CancellationTokenSource

你可以通过配置添加更多需要忽略的类型：

```csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IgnoredTypes.AddIfNotContains(typeof(YourType));
});
```

## 特殊说明

- Elasticsearch 实现支持跨租户，将根据租户自动切换索引
- EntityFrameworkCore 实现主要作为桥梁，具体实现交由 Abp 官方模块管理