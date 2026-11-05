English | [简体中文](./README.md)

# LINGYUN.Abp.OperationRateLimiting

Operation level rate limiting for ABP, implementing the [Operation Rate Limiting](https://github.com/abpframework/abp/blob/dev/docs/en/modules/operation-rate-limiting.md) design document.

Unlike the [ASP.NET Core rate limiting middleware](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit), this module works at the **application/domain code** level: call `IOperationRateLimitingChecker` explicitly or decorate methods/controllers with `[OperationRateLimiting]` to limit specific business operations such as sending SMS codes, generating reports or login attempts.

## Features

* Four built-in algorithms: fixed window, sliding window, token bucket and concurrency (matching ASP.NET Core's built-in limiters).
* Extensible algorithms: implement `IOperationRateLimitingRule`.
* Partitions: parameter, current user, current tenant, client IP, email (upper cased), phone number (formatting removed), custom named resolver.
* Multi-rule policies (AND), with a read-only pre-check phase so a blocked request does not consume quota of the other rules.
* Policy overriding: `AddPolicy` replaces, `ConfigurePolicy` patches, `ClearRules`.
* Declarative usage: `[OperationRateLimiting]` on application services (interceptor) and MVC controllers (action filter).
* Permanent ban policies (`maxCount: 0`), custom error codes, `Properties`.
* Dynamic policies: persist policies in a database and merge them with the static ones through `LINGYUN.Abp.Dynamic.Definitions` (`DynamicDefinitionStrategy`).
* Replaceable store (distributed cache + distributed lock), formatter and policy provider.

## Projects

| Project | Description |
| --- | --- |
| `LINGYUN.Abp.OperationRateLimiting` | Base library: options, policies, algorithms, checker, exception, attributes, localization, default distributed cache store |
| `LINGYUN.Abp.OperationRateLimiting.AspNetCore` | Base library: MVC action filter and client IP resolution |
| `LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis` | Base library: atomic Redis store implemented with Lua scripts |

Policy management (Application / HttpApi / EF Core repository) lives in `modules/operation-rate-limiting`:

| Project | Description |
| --- | --- |
| `LINGYUN.Abp.OperationRateLimiting.Domain.Shared` | Constants, error codes, ETOs such as `OperationRateLimitingPolicyChangedEto`, front-end localization resources |
| `LINGYUN.Abp.OperationRateLimiting.Domain` | Dynamic policy/rule entities and repository abstraction |
| `LINGYUN.Abp.OperationRateLimiting.EntityFrameworkCore` | EF Core repository, cached dynamic policy store, `DbContext` configuration |
| `LINGYUN.Abp.OperationRateLimiting.Application.Contracts` | DTOs, permissions, app service contracts, localization |
| `LINGYUN.Abp.OperationRateLimiting.Application` | Policy CRUD app service (invalidates the dynamic policy cache after writes) |
| `LINGYUN.Abp.OperationRateLimiting.HttpApi` | Policy management REST API |

## Quick start

```csharp
[DependsOn(typeof(AbpOperationRateLimitingModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddPolicy("SendSmsCode", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                      .PartitionByParameter();
            });
        });
    }
}
```

```csharp
public class SmsAppService : ApplicationService
{
    private readonly IOperationRateLimitingChecker _rateLimitChecker;

    public SmsAppService(IOperationRateLimitingChecker rateLimitChecker)
    {
        _rateLimitChecker = rateLimitChecker;
    }

    public virtual async Task SendCodeAsync(string phoneNumber)
    {
        // Throws AbpOperationRateLimitingException (HTTP 429) when the limit is exceeded.
        await _rateLimitChecker.CheckAsync("SendSmsCode", phoneNumber);

        // Send the SMS code...
    }
}
```

| Method | Description |
| --- | --- |
| `CheckAsync` | Checks and increments; throws `AbpOperationRateLimitingException` when denied |
| `IsAllowedAsync` | Read-only check, does not increment |
| `GetStatusAsync` | Returns the detailed status, does not increment |
| `ResetAsync` | Resets the counters of the policy/partition |

For concurrency rules, `AcquireAsync` returns an `IOperationRateLimitingLease` that releases the permit when disposed; `ReleaseAsync` releases it explicitly. String-parameter shortcut extension methods exist for all of them.

## Algorithms

| Builder | Parameters | Semantics |
| --- | --- | --- |
| `WithFixedWindow(duration, maxCount)` | window, limit | At most `maxCount` requests per fixed window |
| `WithSlidingWindow(duration, maxCount, segments = 10)` | window, limit, segments | At most `maxCount` requests over the last window, approximated with aligned segments |
| `WithTokenBucket(period, tokenLimit, tokensPerPeriod = 1)` | replenishment period, bucket size, tokens per period | Starts full, allows bursts, replenishes `tokensPerPeriod` per period |
| `WithConcurrency(permitLimit, leaseDuration)` | permits, lease | At most `permitLimit` in-flight operations; permits are released explicitly or when the lease expires |

The sliding window is a bounded-memory approximation, like ASP.NET Core's `SlidingWindowRateLimiter`.

`Duration` and `MaxCount` are common to every algorithm and live directly on `OperationRateLimitingRuleDefinition`; other algorithm specific values (for example `SlidingWindow:Segments`, `TokenBucket:TokensPerPeriod`) are stored in `OperationRateLimitingRuleDefinition.Properties` with the algorithm name as prefix. Each algorithm namespace provides extension methods for its values (for example `OperationRateLimitingRuleDefinitionTokenBucketExtensions.GetTokensPerPeriod` / `SetTokensPerPeriod`). Custom algorithms can add values with `rule.WithProperty("MyAlgorithm:Key", "value")` and read them with `rule.GetPropertyOrNull("MyAlgorithm:Key")`.

### Redis atomic store (Lua)

The default store uses `IDistributedCache` and guards the read-modify-write with `IAbpDistributedLock`. For Redis deployments reference `LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis` and depend on `AbpOperationRateLimitingStackExchangeRedisModule` to replace it with an **atomic Lua implementation** that needs no distributed lock:

```csharp
[DependsOn(typeof(AbpOperationRateLimitingStackExchangeRedisModule))]
public class MyModule : AbpModule { }
```

```json
{
  "OperationRateLimiting": {
    "Redis": {
      "Configuration": "127.0.0.1:6379",
      "InstanceName": "my-app:"
    }
  }
}
```

* Each built-in algorithm is one Lua script executed inside Redis (fixed window `GET/SET`, sliding window `ZSET`, token bucket `HASH`, concurrency `ZSET`) returning allowed/current/remaining/retry values.
* Custom algorithm states are updated with a Lua compare-and-set loop, so they are atomic as well.
* `ConfigurationOptions` allows advanced connection settings; `InstanceName` prefixes every key.
* Replacing `IOperationRateLimitingStore` is transparent to the rest of the module.

## Partitions

`PartitionByParameter`, `PartitionByCurrentUser`, `PartitionByCurrentTenant`, `PartitionByClientIp`, `PartitionByEmail` (upper cased), `PartitionByPhoneNumber` (punctuation removed, `+` and digits kept), `PartitionBy("Name")` and the inline `PartitionBy("Name", resolver)` overload.

```csharp
options.AddPartitionKeyResolver("ByDevice", ctx =>
    Task.FromResult($"{ctx.Parameter}:{ctx.Properties["DeviceId"]}"));

options.AddPolicy("ApiCall", policy =>
    policy.WithFixedWindow(TimeSpan.FromHours(1), 100).PartitionBy("ByDevice"));
```

## Multi-rule and overriding

```csharp
options.AddPolicy("Login", policy =>
{
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromMinutes(5), 5)
        .PartitionByParameter());

    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromHours(1), 20)
        .PartitionByClientIp());
});
```

All rules must allow the request (AND). Every rule is pre-checked with a read-only operation before any counter is incremented; a failure in the increment phase does not roll back counters incremented by earlier rules (same as the design document).

`AddPolicy` with the same name fully replaces the policy; `ConfigurePolicy` patches it and is pre-populated with the existing rules; `RemovePolicy` deletes it. `WithName("...")` gives a rule a stable counter key so changes to its duration/max count do not reset counters.

## Declarative usage

```csharp
public class SmsAppService : ApplicationService
{
    [OperationRateLimiting("SendSmsCode")]
    public virtual async Task SendCodeAsync([RateLimitingParameter] string phoneNumber)
    {
        // ...
    }
}
```

```csharp
[HttpPost("send-sms-code")]
[OperationRateLimiting("SendSmsCode")]
public async Task<IActionResult> SendSmsCodeAsync([RateLimitingParameter] string phoneNumber)
{
    return Ok();
}
```

The partition parameter is resolved from (1) a `[RateLimitingParameter]` parameter, (2) a parameter implementing `IHasOperationRateLimitingParameter`, or (3) `null`. Marking more than one parameter throws an `AbpException`. A class-level attribute applies to all public methods; a method-level attribute has precedence.

## Exception

`AbpOperationRateLimitingException` is a `BusinessException` with HTTP status code **429**. Error codes: `Volo.Abp.OperationRateLimiting:010001` (regular) and `Volo.Abp.OperationRateLimiting:010002` (permanent ban); a policy or rule `WithErrorCode()` overrides them. `Data` contains `PolicyName`, `MaxCount`, `CurrentCount`, `RemainingCount`, `RetryAfterSeconds`, `RetryAfterMinutes`, `RetryAfter` (localized, absent for bans), `WindowDurationSeconds`, `WindowDescription`, `RuleDetails` and every `OperationRateLimitingContext.Properties` entry.

## Multi-tenancy

Counters are shared across tenants by default. `WithMultiTenancy()` adds the current tenant to the cache key:

* Global: `orl:{PolicyName}:{RuleKey}:{PartitionKey}`
* Tenant: `orl:t:{TenantId}:{PolicyName}:{RuleKey}:{PartitionKey}`

Partition values appear in cache keys as-is; do not use secrets as partition values.

## Dynamic policies (database, in modules/operation-rate-limiting)

Dynamic policy persistence, app services and the REST API live in `modules/operation-rate-limiting`; the base library keeps only the `IDynamicOperationRateLimitingPolicyStore` abstraction and the merge logic. Reference `LINGYUN.Abp.OperationRateLimiting.EntityFrameworkCore` and depend on `AbpOperationRateLimitingEntityFrameworkCoreModule`.

```csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure<OperationRateLimitingDbContext>(context =>
    {
        context.DbContextOptions.UseSqlServer(connectionString);
    });
});
```

Or add the entities to an existing `DbContext` (two tables: `OperationRateLimitingPolicies` and `OperationRateLimitingPolicyRules`) and call `builder.ConfigureOperationRateLimiting()`.

```csharp
var record = new OperationRateLimitingPolicyRecord(GuidGenerator.Create(), "DynamicPolicy", CurrentTenant.Id);
record.SetRules(new[]
{
    new OperationRateLimitingRuleRecord(GuidGenerator.Create(), record.Id, OperationRateLimitingAlgorithms.FixedWindow)
    {
        Duration = TimeSpan.FromMinutes(1),
        MaxCount = 5,
        PartitionType = OperationRateLimitingPartitionType.Parameter
    }
});
await _policyRecordRepository.InsertAsync(record);

await _dynamicPolicyStore.InvalidateCacheAsync();
```

Static and database policies are provided by `IStaticOperationRateLimitingPolicyStore` and `IDynamicOperationRateLimitingPolicyStore` and merged by `IOperationRateLimitingPolicyManager` (default implementation `OperationRateLimitingPolicyManager`) through `LINGYUN.Abp.Dynamic.Definitions` (`DynamicDefinitionStrategy.Merge` by default):

* `IStaticOperationRateLimitingPolicyStore` defaults to `OptionsOperationRateLimitingPolicyStore`, which reads `AbpOperationRateLimitingOptions.Policies`; replace it to load static policies from configuration, files, etc.
* `IDynamicOperationRateLimitingPolicyStore` defaults to an empty store; referencing the management EF Core project replaces it with `EfCoreOperationRateLimitingPolicyStore`.

For a policy defined in both places, the database `DisplayName`, `ErrorCode` and `IsEnabled` override the static values and a non-empty database rule list replaces the static rules. Dynamic definitions are cached for 5 minutes per tenant.

## Extensibility

* **Algorithm**: implement `IOperationRateLimitingRule`; register with `options.AddAlgorithm<T>("Name")` or add it directly with `policy.AddRule<T>()`. Implement `IOperationRateLimitingRuleReleaser` to support permit release (see the concurrency rule).
* **Store**: implement `IOperationRateLimitingStore`. The interface is algorithm oriented: `ExecuteAsync(OperationRateLimitingStoreRequest)` executes the four built-in algorithms atomically, `UpdateStateAsync<TState>` serves custom algorithms, `ReleaseAsync` releases concurrency permits and `ResetAsync` removes a counter. Implementations must keep `ExecuteAsync` and `UpdateStateAsync` atomic (distributed lock, Redis Lua or database atomic statements); see `StackExchangeRedisOperationRateLimitingStore`.
* **Formatter**: implement `IOperationRateLimitingFormatter` to customize duration texts.
* **Policy provider**: implement `IOperationRateLimitingPolicyProvider` to fully replace the policy source.
* **Static policy store**: implement `IStaticOperationRateLimitingPolicyStore` to load static policies from configuration, files, etc. The default `OptionsOperationRateLimitingPolicyStore` reads `AbpOperationRateLimitingOptions`.
* **Dynamic policy store**: implement `IDynamicOperationRateLimitingPolicyStore` (database, remote configuration, ...); `AbpDynamicDefinitionsOptions.MapStrategy<OperationRateLimitingPolicy>()` controls how it is merged with the static policies.

## Options

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.IsEnabled = true;
    options.LockTimeout = TimeSpan.FromSeconds(5);
});
```

When globally disabled, `CheckAsync`/`ResetAsync` do nothing, `IsAllowedAsync` returns `true` and `GetStatusAsync` returns an allowed status with `int.MaxValue` limits and `CurrentCount = 0`.

## Deployment notes

* Single instance works out of the box (ABP's `LocalAbpDistributedLock` is used by default), or use the Redis store for atomicity without a lock.
* Multi-instance deployments must use the Redis store, or replace `IAbpDistributedLock` with a real distributed lock provider and use a distributed cache (e.g. Redis) for the counters.
* Policies are stored in the `OperationRateLimitingPolicies` table and their rules in `OperationRateLimitingPolicyRules` (the `Properties` column is JSON); generate a migration for your database provider.
