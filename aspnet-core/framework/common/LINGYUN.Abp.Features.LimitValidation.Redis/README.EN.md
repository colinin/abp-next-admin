# LINGYUN.Abp.Features.LimitValidation.Redis

Redis-based feature limit validation component

## Features

* Use Redis to store and validate feature call count limits
* Support Lua scripts for atomic operations
* Support multiple limitation policies (minute, hour, day, week, month, year)
* Support custom Redis configuration options

## Configuration and Usage

1. Add module dependency

```csharp
[DependsOn(typeof(AbpFeaturesValidationRedisModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. Configure Redis options

```json
{
  "Features": {
    "Validation": {
      "Redis": {
        "Configuration": "127.0.0.1",
        "InstanceName": "YourInstanceName"
      }
    }
  }
}
```

## Configuration Options

* Configuration: Redis connection string
* InstanceName: Redis instance name (optional)
* ConfigurationOptions: Redis configuration options (optional, for more detailed Redis configuration)

## Usage Example

```csharp
// Limit a method to be called at most 100 times per minute
[RequiresLimitFeature("YourFeature.MethodLimit", "YourFeature.Interval", LimitPolicy.Minute)]
public async Task YourMethod()
{
    // Business logic
}
```

[简体中文](./README.md) | [English](./README.EN.md)
