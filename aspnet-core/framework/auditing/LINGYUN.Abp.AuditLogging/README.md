# LINGYUN.Abp.AuditLogging

审计日志核心模块，提供审计日志的基础功能和接口定义。

[English](./README.EN.md)

## 功能特性

* 审计日志基础设施
* 审计日志仓储接口定义
* 审计日志管理器接口定义
* 支持忽略特定类型的审计日志记录

## 模块引用

```csharp
[DependsOn(typeof(AbpAuditLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Auditing": {
    "IsEnabled": true,  // 是否启用审计日志
    "HideErrors": true, // 是否隐藏错误信息
    "IsEnabledForAnonymousUsers": true, // 是否为匿名用户启用审计日志
    "IsEnabledForGetRequests": false,   // 是否为GET请求启用审计日志
    "ApplicationName": null  // 应用程序名称
  }
}
```

## 基本用法

1. 引用模块
2. 配置审计日志选项
3. 实现审计日志存储提供者（例如：EntityFrameworkCore或Elasticsearch）

## 高级功能

### 忽略特定类型

默认情况下，模块会忽略以下类型的审计日志：
* CancellationToken
* CancellationTokenSource

你可以通过配置添加更多需要忽略的类型：

```csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IgnoredTypes.AddIfNotContains(typeof(YourType));
});
```
