# LINGYUN.Abp.Hangfire.Storage.MySql

English | [简体中文](README.md)

## 1. Introduction

`LINGYUN.Abp.Hangfire.Storage.MySql` is an ABP module for configuring Hangfire to use MySQL as storage. This module provides a simple configuration approach to easily use MySQL as Hangfire's persistent storage.

## 2. Features

* Simple MySQL storage configuration
* Custom connection string support
* MySQL storage options configuration
* Seamless integration with ABP configuration system

## 3. Installation

```bash
dotnet add package LINGYUN.Abp.Hangfire.Storage.MySql
```

## 4. Usage

1. Add `AbpHangfireMySqlStorageModule` to your module dependencies:

```csharp
[DependsOn(typeof(AbpHangfireMySqlStorageModule))]
public class YourModule : AbpModule
{
}
```

2. Configure connection string and storage options in appsettings.json:

```json
{
  "Hangfire": {
    "MySql": {
      "Connection": "Server=localhost;Database=YourDb;Uid=root;Pwd=123456;",
      "TablePrefix": "Hangfire",
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
1. `Hangfire:MySql:Connection`
2. `ConnectionStrings:Default`

### 5.2 Storage Options

* `TablePrefix`: Table prefix for database tables
* `CommandBatchMaxTimeout`: Maximum timeout for command batches
* `SlidingInvisibilityTimeout`: Sliding invisibility timeout
* `QueuePollInterval`: Queue polling interval
* `UseRecommendedIsolationLevel`: Whether to use recommended isolation level
* `DisableGlobalLocks`: Whether to disable global locks

## 6. Dependencies

* Volo.Abp.Hangfire
* Hangfire.MySql.Core

## 7. Documentation and Resources

* [Hangfire Official Documentation](https://docs.hangfire.io/)
* [Hangfire.MySql.Core Documentation](https://github.com/arnoldasgudas/Hangfire.MySqlStorage)
* [ABP Framework Documentation](https://docs.abp.io/)
