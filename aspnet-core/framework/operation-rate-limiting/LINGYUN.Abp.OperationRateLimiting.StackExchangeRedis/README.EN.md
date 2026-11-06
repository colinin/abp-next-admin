# LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis

[简体中文](./README.md) | English

Atomic Redis store of the operation rate limiting module. Each built-in algorithm is executed by a single Lua script inside Redis (atomic read-modify-write, no distributed lock); custom algorithm states are updated with a Lua compare-and-set loop.

## Usage

```csharp
[DependsOn(typeof(AbpOperationRateLimitingStackExchangeRedisModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

```json
{
  "OperationRateLimiting": {
    "Redis": {
      "Configuration": "127.0.0.1:6379",
      "InstanceName": "your-app:"
    }
  }
}
```

Or configure the options directly:

```csharp
Configure<AbpOperationRateLimitingRedisOptions>(options =>
{
    options.ConfigurationOptions = ConfigurationOptions.Parse("127.0.0.1:6379");
    options.InstanceName = "your-app:";
});
```

The module replaces the default `DistributedCacheOperationRateLimitingStore` with `StackExchangeRedisOperationRateLimitingStore`; `IOperationRateLimitingChecker` callers need no change.

## Options

| Option | Description |
| --- | --- |
| `Configuration` | StackExchange.Redis connection string, default `127.0.0.1:6379` |
| `ConfigurationOptions` | Advanced connection options; ignores `Configuration` when set |
| `InstanceName` | Optional prefix of every Redis key |

## Lua scripts

| Script | Data structure | Description |
| --- | --- | --- |
| `fixed-window.lua` | String (count) + PX | Aligned window, increments within the window and expires at the window end |
| `sliding-window.lua` | ZSET (request timestamps) | Exact sliding window; expired members are removed with `ZREMRANGEBYSCORE` |
| `token-bucket.lua` | HASH (tokens/last) | Replenishes tokens per period and returns the next available time |
| `concurrency.lua` | ZSET (lease start times) | Removes expired leases then acquires a permit; `concurrency-release.lua` releases the oldest lease |

The sliding window and concurrency sets hold at most `maxCount` members (members are only added while the limit is not exceeded), so memory is bounded by the policy limit.

## Notes

* Scripts touch a single key, so they are Redis Cluster compatible.
* Custom algorithms use `IOperationRateLimitingStore.UpdateStateAsync`; its CAS loop retries up to 500 times under contention while keeping the final count accurate.
* Ban policies (`maxCount: 0`) are returned before touching Redis, so no key is created.
