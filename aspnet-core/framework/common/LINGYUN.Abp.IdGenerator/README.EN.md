# LINGYUN.Abp.IdGenerator

## Introduction

`LINGYUN.Abp.IdGenerator` is a distributed ID generator module that implements the Snowflake algorithm to generate distributed unique IDs.

## Features

* Snowflake Algorithm ID Generator (`SnowflakeIdGenerator`)
  * Support for custom worker ID and datacenter ID
  * Support for custom sequence bits
  * Support for time rollback handling
  * Provides unique ID generation in distributed environments

## Configuration

### SnowflakeIdOptions

* `WorkerIdBits` (default: 5) - Number of bits for worker ID
* `DatacenterIdBits` (default: 5) - Number of bits for datacenter ID
* `Sequence` (default: 0) - Initial value for sequence
* `SequenceBits` (default: 12) - Number of bits for sequence
* `UsePreviousInTimeRollback` (default: true) - Whether to use previous timestamp when time rolls back
* `WorkerId` - Worker ID, if not specified, gets from environment variable WORKERID or generates randomly
* `DatacenterId` - Datacenter ID, if not specified, gets from environment variable DATACENTERID or generates randomly

## Installation

```bash
dotnet add package LINGYUN.Abp.IdGenerator
```

## Usage

1. Add `[DependsOn(typeof(AbpIdGeneratorModule))]` to your module class.

```csharp
[DependsOn(typeof(AbpIdGeneratorModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SnowflakeIdOptions>(options =>
        {
            options.WorkerId = 1;
            options.DatacenterId = 1;
        });
    }
}
```

2. Inject and use the ID generator:

```csharp
public class YourService
{
    private readonly IDistributedIdGenerator _idGenerator;

    public YourService(IDistributedIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public long CreateId()
    {
        return _idGenerator.Create();
    }
}
```

## Links

* [中文文档](./README.md)
