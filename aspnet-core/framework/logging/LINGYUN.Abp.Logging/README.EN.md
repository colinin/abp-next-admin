# LINGYUN.Abp.Logging

Basic logging module

Defines the ILoggingManager interface for implementing log information queries.

[简体中文](./README.md)

## Features

* Provides unified logging query interface ILoggingManager
* Supports various log field queries, including timestamp, log level, message, etc.
* Supports exception information recording and querying
* Supports rich log field information such as machine name, environment name, application name, etc.
* Supports pagination query and count statistics

## Module Reference

```csharp
[DependsOn(typeof(AbpLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

Configure in `appsettings.json`:

```json
{
  "Logging": {
    "MachineName": "your-machine-name",
    "EnvironmentName": "environment-name"
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
    level: LogLevel.Error
);

// Get log count
var count = await _loggingManager.GetCountAsync(
    startTime: DateTime.Now.AddDays(-1),
    level: LogLevel.Error
);

// Get single log
var log = await _loggingManager.GetAsync(id);
```

## Log Field Description

* TimeStamp - Log timestamp
* Level - Log level
* Message - Log message
* Fields - Log field information
  * Id - Log unique identifier
  * MachineName - Machine name
  * Environment - Environment name
  * Application - Application name
  * Context - Context
  * ActionId - Action ID
  * ActionName - Action name
  * RequestId - Request ID
  * RequestPath - Request path
  * ConnectionId - Connection ID
  * CorrelationId - Correlation ID
  * ClientId - Client ID
  * UserId - User ID
  * ProcessId - Process ID
  * ThreadId - Thread ID
* Exceptions - Exception information list
