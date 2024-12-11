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
  }
}
```

### Elasticsearch 配置

```json
{
  "AuditLogging": {
    "Elasticsearch": {
      "IndexPrefix": "auditlogging" // 索引前缀
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
