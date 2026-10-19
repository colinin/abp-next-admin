# LINGYUN.Abp 日志模块

## 模块概述

日志模块提供日志接入与检索能力: 基于 Serilog 的日志写入与属性增强, 以及从持久化设施中检索日志的
`ILoggingProvider` 实现（通过 `ILoggingManager` 对外提供统一查询接口）。

## 模块列表

| 模块 | 说明 | 文档 |
| --- | --- | --- |
| LINGYUN.Abp.Logging | 日志基础设施, 定义并实现 `ILoggingManager`, 实现日志信息查询 | [README](./LINGYUN.Abp.Logging/README.md) |
| LINGYUN.Abp.Logging.Serilog.Elasticsearch | `ILoggingProvider` 的 Elasticsearch 实现 | [README](./LINGYUN.Abp.Logging.Serilog.Elasticsearch/README.md) |
| LINGYUN.Abp.Logging.Serilog.OpenObserve | `ILoggingProvider` 的 OpenObserve 实现 | [README](./LINGYUN.Abp.Logging.Serilog.OpenObserve/README.md) |
| LINGYUN.Abp.Serilog.Enrichers.Application | 为日志属性添加应用程序标识 | [README](./LINGYUN.Abp.Serilog.Enrichers.Application/README.md) |
| LINGYUN.Abp.Serilog.Enrichers.UniqueId | 为日志属性添加唯一标识（雪花算法） | [README](./LINGYUN.Abp.Serilog.Enrichers.UniqueId/README.md) |

## 功能特性

### 核心功能

- 统一的日志查询接口 `ILoggingManager`, 以及可替换的 `ILoggingProvider` 抽象
- 支持多种日志字段查询（时间戳、日志级别、消息等）
- 支持异常信息记录和查询
- 支持丰富的日志字段信息（机器名称、环境名称、应用名称、请求上下文等）
- 支持分页查询和计数统计

### 存储支持

- Elasticsearch 实现（Serilog 写入 + `ILoggingProvider` 检索）
- OpenObserve 实现（Serilog 写入 + `ILoggingProvider` 检索）

> 两种实现都通过 `Logging:Provider` 指定检索实现。

### 日志属性增强

- 应用名称（`WithApplicationName`）
- 唯一标识（`WithUniqueId`, 雪花算法）

## 模块引用

### 核心模块

```csharp
[DependsOn(typeof(AbpLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

### Elasticsearch 模块

```csharp
[DependsOn(typeof(AbpLoggingSerilogElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

### OpenObserve 模块

```csharp
[DependsOn(typeof(AbpLoggingSerilogOpenObserveModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

### 应用名称增强器

```csharp
[DependsOn(typeof(AbpSerilogEnrichersApplicationModule))]
public class YouProjectModule : AbpModule
{
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
    AbpSerilogEnrichersConsts.ApplicationName = "demo-app";
  }
}
```

### 唯一标识增强器

```csharp
[DependsOn(typeof(AbpSerilogEnrichersUniqueIdModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置选项

### 日志基础配置

```json
{
  "Logging": {
    "MachineName": "你的机器名称",
    "EnvironmentName": "环境名称"
  }
}
```

### Elasticsearch 配置

* AbpLoggingSerilogElasticsearchOptions.IndexFormat  必须和 Serilog 配置项中的 IndexFormat 相同, 否则无法定位到正确的索引

```json
{
  "Logging": {
    "Provider": "ElasticsearchLoggingProvider",
    "Serilog": {
      "Elasticsearch": {
        "IndexFormat": "logstash-{0:yyyy.MM.dd}"
      }
    }
  }
}
```

### OpenObserve 配置

* AbpLoggingSerilogOpenObserveOptions.Stream  存储日志的 stream 名称
* AbpLoggingSerilogOpenObserveOptions.Organization  存储日志的 organization 名称

```json
{
  "Logging": {
    "Provider": "OpenObserveLoggingProvider",
    "Serilog": {
      "OpenObserve": {
        "Stream": "default",
        "Organization": "default"
      }
    }
  },
  "OpenObserve": {
    "Endpoint": "http://localhost:5080",
    "AccessToken": "Basic XXXXXXXXXXX"
  }
}
```

### 唯一标识配置

* AbpSerilogUniqueIdConsts.UniqueIdPropertyName  唯一标识字段的名称, 默认为 "UniqueId"

```json
{
  "UniqueId": {
    "Snowflake": {
      "WorkerId": 1,        // 工作机器ID
      "DatacenterId": 1,    // 数据中心ID
      "Sequence": 0,        // 序列号
      "BaseTime": "2020-01-01 00:00:00"  // 基准时间
    }
  }
}
```

### 日志属性增强（Serilog Enrich）

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "Enrich": [ "WithApplicationName", "WithUniqueId" ]
  }
}
```

## 基本用法

1. 注入 `ILoggingManager` 接口：

```csharp
public class YourService
{
    private readonly ILoggingManager _loggingManager;

    public YourService(ILoggingManager loggingManager)
    {
        _loggingManager = loggingManager;
    }
}
```

2. 查询日志：

```csharp
// 获取日志列表
var logs = await _loggingManager.GetListAsync(
    startTime: DateTime.Now.AddDays(-1),
    maxResultCount: 10,
    skipCount: 0,
    level: LogLevel.Error,
    environment: "Production",
    application: "YourApp"
);

// 获取日志总数
var count = await _loggingManager.GetCountAsync(
    startTime: DateTime.Now.AddDays(-1),
    level: LogLevel.Error
);

// 获取单条日志
var log = await _loggingManager.GetAsync(id);
```

### 支持的查询条件

* 时间范围（startTime、endTime）
* 日志级别（level）
* 机器名称（machineName）
* 环境名称（environment）
* 应用名称（application）
* 上下文（context）
* 请求ID（requestId）
* 请求路径（requestPath）
* 关联ID（correlationId）
* 进程ID（processId）
* 线程ID（threadId）
* 是否包含异常（hasException）

## 日志字段说明

* TimeStamp - 日志时间戳
* Level - 日志级别
* Message - 日志消息
* Fields - 日志字段信息
  * Id - 日志唯一标识
  * MachineName - 机器名称
  * Environment - 环境名称
  * Application - 应用名称
  * Context - 上下文
  * ActionId - 操作ID
  * ActionName - 操作名称
  * RequestId - 请求ID
  * RequestPath - 请求路径
  * ConnectionId - 连接ID
  * CorrelationId - 关联ID
  * ClientId - 客户端ID
  * UserId - 用户ID
  * ProcessId - 进程ID
  * ThreadId - 线程ID
* Exceptions - 异常信息列表

## 特殊说明

- Elasticsearch 实现: `IndexFormat` 必须与 Serilog 配置保持一致（默认 `logstash-{0:yyyy.MM.dd}`）;
  支持多租户, 会根据当前租户ID过滤日志; 查询支持分页、排序与请求路径模糊匹配
- OpenObserve 实现: 支持按 `Stream`/`Organization` 检索; 写入侧受服务端 `ZO_INGEST_ALLOWED_UPTO`
  （默认 5 小时）限制; 存储为 append-only, 不支持按记录删除
- 应用名称增强器: 应用名称为静态值, 一旦设置对该应用的所有日志条目生效; 仅当日志事件不存在该属性时才写入
- 唯一标识增强器: 雪花算法生成的 ID 趋势递增但不保证严格递增; 工作机器ID与数据中心ID在集群中必须唯一;
  基准时间设置后不可修改

## 相关模块

* 审计日志的 OpenObserve 实现: [LINGYUN.Abp.AuditLogging.OpenObserve](../auditing/LINGYUN.Abp.AuditLogging.OpenObserve/README.md)
* 审计日志的 Elasticsearch 实现: [LINGYUN.Abp.AuditLogging.Elasticsearch](../auditing/LINGYUN.Abp.AuditLogging.Elasticsearch/README.md)
