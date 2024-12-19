# LINGYUN.Abp.Hangfire.Storage.SqlServer

English | [简体中文](README.md)

## 1. Introduction

`LINGYUN.Abp.Hangfire.Storage.SqlServer` is an ABP module for configuring Hangfire to use SQL Server as storage. This module provides a simple configuration approach to easily use SQL Server as Hangfire's persistent storage.

## 2. Features

* Simple SQL Server storage configuration
* Custom connection string support
* SQL Server storage options configuration
* Seamless integration with ABP configuration system

## 3. Installation

```bash
dotnet add package LINGYUN.Abp.Hangfire.Storage.SqlServer
```

## 4. Usage

1. Add `AbpHangfireSqlServerStorageModule` to your module dependencies:

```csharp
[DependsOn(typeof(AbpHangfireSqlServerStorageModule))]
public class YourModule : AbpModule
{
}
```

2. Configure connection string and storage options in appsettings.json:

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

## 5. Configuration

### 5.1 Connection String Configuration

The module looks for connection string in the following order:
1. `Hangfire:SqlServer:Connection`
2. `ConnectionStrings:Default`

### 5.2 Storage Options

* `CommandBatchMaxTimeout`: Maximum timeout for command batches
* `SlidingInvisibilityTimeout`: Sliding invisibility timeout
* `QueuePollInterval`: Queue polling interval
* `UseRecommendedIsolationLevel`: Whether to use recommended isolation level
* `DisableGlobalLocks`: Whether to disable global locks

## 6. Dependencies

* Volo.Abp.Hangfire
* Hangfire.SqlServer

## 7. Documentation and Resources

* [Hangfire Official Documentation](https://docs.hangfire.io/)
* [Hangfire.SqlServer Documentation](https://docs.hangfire.io/en/latest/configuration/using-sql-server.html)
* [ABP Framework Documentation](https://docs.abp.io/)
