# LINGYUN.Abp.Logging

日志基础模块

定义 ILoggingManager 接口, 实现日志信息查询  

[English](./README.EN.md)

## 功能特性

* 提供统一的日志查询接口 ILoggingManager
* 支持多种日志字段查询，包括时间戳、日志级别、消息等
* 支持异常信息记录和查询
* 支持丰富的日志字段信息，如机器名称、环境名称、应用名称等
* 支持分页查询和计数统计

## 模块引用

```csharp
[DependsOn(typeof(AbpLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

在 `appsettings.json` 中配置：

```json
{
  "Logging": {
    "MachineName": "你的机器名称",
    "EnvironmentName": "环境名称"
  }
}
```

## 基本用法

1. 注入 ILoggingManager 接口：
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
    level: LogLevel.Error
);

// 获取日志总数
var count = await _loggingManager.GetCountAsync(
    startTime: DateTime.Now.AddDays(-1),
    level: LogLevel.Error
);

// 获取单条日志
var log = await _loggingManager.GetAsync(id);
```

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
