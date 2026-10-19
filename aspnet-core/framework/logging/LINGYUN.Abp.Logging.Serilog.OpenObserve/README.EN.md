# LINGYUN.Abp.Logging.Serilog.OpenObserve

[简体中文](./README.md) | English

An OpenObserve implementation of `ILoggingProvider` that retrieves log entries stored in OpenObserve.

## Features

* Log storage and retrieval on top of OpenObserve
* Support for all log levels (Debug, Information, Warning, Error, Critical)
* Automatic mapping between Serilog levels and `Microsoft.Extensions.Logging` levels
* Rich query conditions (time range, level, machine name, environment, application, ...)
* Storage and retrieval of exception details
* Multi-tenancy support

## Module reference

```csharp
[DependsOn(typeof(AbpLoggingSerilogOpenObserveModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Options

* AbpLoggingSerilogOpenObserveOptions.Stream — name of the stream that stores the logs
* AbpLoggingSerilogOpenObserveOptions.Organization — name of the organization that stores the logs

## appsettings.json

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

## Usage

1. Inject `ILoggingManager`:

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
// get a list of logs
var logs = await _loggingManager.GetListAsync(
    startTime: DateTime.Now.AddDays(-1),
    maxResultCount: 10,
    skipCount: 0,
    level: LogLevel.Error,
    environment: "Production",
    application: "YourApp"
);

// get the total count
var count = await _loggingManager.GetCountAsync(
    startTime: DateTime.Now.AddDays(-1),
    level: LogLevel.Error
);

// get a single log entry
var log = await _loggingManager.GetAsync(id);
```

## Supported query conditions

* Time range (startTime, endTime)
* Log level (level)
* Machine name (machineName)
* Environment (environment)
* Application (application)
* Context (context)
* Request id (requestId)
* Request path (requestPath)
* Correlation id (correlationId)
* Process id (processId)
* Thread id (threadId)
* Has exception (hasException)

## Shared components

* Connection and serialization: `LINGYUN.Abp.OpenObserve` (`OpenObserveClient`, `IOpenObserveSerializer`, `AbpOpenObserveOptions`)
* Query conditions and SQL building: `ExpressionParser`, `Condition`, `ComparisonOp` and `OpenObserveSqlBuilder`
  under `LINGYUN.Abp.OpenObserve.Utils`

## Related modules

* OpenObserve implementation for audit logs: [LINGYUN.Abp.AuditLogging.OpenObserve](../../auditing/LINGYUN.Abp.AuditLogging.OpenObserve/README.EN.md)
