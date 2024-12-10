# LINGYUN.Abp.Logging.Serilog.Elasticsearch

[简体中文](./README.md) | English

Elasticsearch implementation of the ILoggingManager interface, retrieving log information from Elasticsearch.

## Features

* Elasticsearch-based log storage and retrieval
* Support for various log levels (Debug, Information, Warning, Error, Critical)
* Automatic mapping between Serilog and Microsoft.Extensions.Logging log levels
* Rich query conditions (time range, log level, machine name, environment name, etc.)
* Exception information storage and retrieval
* Multi-tenancy support

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLoggingSerilogElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Options

* AbpLoggingSerilogElasticsearchOptions.IndexFormat - Must match the IndexFormat in Serilog configuration, otherwise the correct index cannot be located

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

## Basic Usage

1. Inject ILoggingManager interface:
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

2. Query logs:
```csharp
// Get log list
var logs = await _loggingManager.GetListAsync(
    startTime: DateTime.Now.AddDays(-1),
    maxResultCount: 10,
    skipCount: 0,
    level: LogLevel.Error,
    environment: "Production",
    application: "YourApp"
);

// Get log count
var count = await _loggingManager.GetCountAsync(
    startTime: DateTime.Now.AddDays(-1),
    level: LogLevel.Error
);

// Get single log
var log = await _loggingManager.GetAsync(id);
```

## Supported Query Conditions

* Time range (startTime, endTime)
* Log level (level)
* Machine name (machineName)
* Environment name (environment)
* Application name (application)
* Context (context)
* Request ID (requestId)
* Request path (requestPath)
* Correlation ID (correlationId)
* Process ID (processId)
* Thread ID (threadId)
* Has exception (hasException)

## Important Notes

1. The IndexFormat configuration must be consistent between your Serilog settings and this module's configuration. The default format is "logstash-{0:yyyy.MM.dd}".
2. Multi-tenancy support with automatic filtering based on current tenant ID.
3. Query results support pagination and sorting.
4. Support for fuzzy matching of request paths.
5. Support for existence query of exception information.

## Object Mapping

* SerilogInfo to LogInfo
  - Automatic mapping of timestamp and log level
  - Proper conversion between Serilog and Microsoft.Extensions.Logging log levels
* SerilogException to LogException
  - Maps exception details including message, type, and stack trace
* SerilogField to LogField
  - Maps all log fields including machine name, environment, application name, etc.
  - Supports unique ID generation for log entries
