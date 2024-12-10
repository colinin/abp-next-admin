# LINGYUN.Abp.Features.LimitValidation.Redis.Client

Redis-based client feature limit validation component

## Features

* Inherits all functionality from LINGYUN.Abp.Features.LimitValidation.Redis
* Specifically designed for client application feature limit validation
* Support client-specific limitation policies

## Configuration and Usage

1. Add module dependency

```csharp
[DependsOn(typeof(AbpFeaturesValidationRedisClientModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. Configure Redis options (same as LINGYUN.Abp.Features.LimitValidation.Redis)

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

## Usage Example

```csharp
// Limit a client feature to be called at most 1000 times per day
[RequiresLimitFeature("YourClientFeature.DailyLimit", "YourClientFeature.Interval", LimitPolicy.Days)]
public async Task YourClientMethod()
{
    // Client business logic
}
```

[简体中文](./README.md) | [English](./README.EN.md)
