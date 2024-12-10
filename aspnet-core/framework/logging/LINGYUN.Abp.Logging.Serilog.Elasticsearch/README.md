# LINGYUN.Abp.Logging.Serilog.Elasticsearch

简体中文 | [English](./README.EN.md)

ILoggingManager 接口的ES实现, 从ES中检索日志信息  

## 功能特性

* 基于Elasticsearch的日志存储和检索
* 支持多种日志级别（Debug、Information、Warning、Error、Critical）
* 支持Serilog和Microsoft.Extensions.Logging日志级别的自动映射
* 支持丰富的查询条件（时间范围、日志级别、机器名称、环境名称等）
* 支持异常信息的存储和检索
* 支持多租户

## 模块引用

```csharp
[DependsOn(typeof(AbpLoggingSerilogElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*  AbpLoggingSerilogElasticsearchOptions.IndexFormat	必须和Serilog配置项中的IndexFormat相同,否则无法定位到正确的索引  

## appsettings.json

```json
{
  "Logging": {
    "Serilog": {
      "Elasticsearch": {
        "IndexFormat": "logstash-{0:yyyy.MM.dd}"
      }
    }
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

## 支持的查询条件

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

## 注意事项

1. IndexFormat配置必须与Serilog配置保持一致，默认格式为"logstash-{0:yyyy.MM.dd}"
2. 支持多租户，会自动根据当前租户ID过滤日志
3. 查询结果支持分页和排序
4. 支持请求路径的模糊匹配
5. 支持异常信息的存在性查询