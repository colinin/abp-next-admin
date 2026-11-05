English | [简体中文](./README.md)

# Operation rate limiting policy management (modules/operation-rate-limiting)

The operation rate limiting base libraries live in `framework/operation-rate-limiting`; this module persists dynamic policies and exposes them through application services and a REST API.

## Layers

| Project | Description |
| --- | --- |
| `LINGYUN.Abp.OperationRateLimiting.Domain.Shared` | Constants, error codes, ETOs such as `OperationRateLimitingPolicyChangedEto`, front-end localization resources |
| `LINGYUN.Abp.OperationRateLimiting.Domain` | `OperationRateLimitingPolicyRecord` and `OperationRateLimitingRuleRecord` entities, `IOperationRateLimitingPolicyRecordRepository`, entity/runtime mapping extensions |
| `LINGYUN.Abp.OperationRateLimiting.EntityFrameworkCore` | EF Core repository, `OperationRateLimitingDbContext`, `ConfigureOperationRateLimiting()`, cached dynamic policy store |
| `LINGYUN.Abp.OperationRateLimiting.Application.Contracts` | DTOs, permissions, `IOperationRateLimitingPolicyAppService`, localization |
| `LINGYUN.Abp.OperationRateLimiting.Application` | Policy CRUD app service; invalidates the dynamic policy cache after writes |
| `LINGYUN.Abp.OperationRateLimiting.HttpApi` | `api/operation-rate-limiting/policies` REST API |

## Registration

```csharp
[DependsOn(
    typeof(AbpOperationRateLimitingEntityFrameworkCoreModule),
    typeof(AbpOperationRateLimitingManagementApplicationModule),
    typeof(AbpOperationRateLimitingManagementHttpApiModule))]
public class YouProjectModule : AbpModule { }
```

```csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure<OperationRateLimitingDbContext>(context =>
    {
        context.DbContextOptions.UseSqlServer(connectionString);
    });
});
```

Or add `OperationRateLimitingPolicyRecord` / `OperationRateLimitingRuleRecord` to an existing `DbContext` and call `builder.ConfigureOperationRateLimiting()` from `OnModelCreating` (policy table `OperationRateLimitingPolicies`, rule child table `OperationRateLimitingPolicyRules` with a JSON `Properties` column); generate a migration for your provider.

## Permissions

`OperationRateLimitingManagement.Policies` (read) plus `.Create`, `.Update`, `.Delete`.

## REST API

| Method | Route | Description |
| --- | --- | --- |
| GET | `api/operation-rate-limiting/policies/{id}` | Get by id |
| GET | `api/operation-rate-limiting/policies/by-name/{name}` | Get by name |
| GET | `api/operation-rate-limiting/policies` | Paged list (`Filter` / `IsEnabled` / `Algorithm`) |
| POST | `api/operation-rate-limiting/policies` | Create |
| PUT | `api/operation-rate-limiting/policies/{id}` | Update |
| DELETE | `api/operation-rate-limiting/policies/{id}` | Delete |

```json
{
  "name": "SendSmsCode",
  "displayName": "Send SMS code",
  "isEnabled": true,
  "rules": [
    {
      "algorithm": "FixedWindow",
      "durationSeconds": 60,
      "maxCount": 1,
      "partitionType": "Parameter"
    }
  ]
}
```

Rule fields: `algorithm` (`FixedWindow`, `SlidingWindow`, `TokenBucket`, `Concurrency` or a custom algorithm registered at startup), `durationSeconds`, `maxCount`, `segments`, `tokensPerPeriod`, `partitionType`, `partitionResolverName`, `isMultiTenancyEnabled`, `errorCode`, `name` (stable counter key), `properties` (algorithm specific values such as `SlidingWindow:Segments` or `TokenBucket:TokensPerPeriod`). `durationSeconds` and `maxCount` are common properties stored directly on the rule definition.

Partition types: `Parameter`, `CurrentUser`, `CurrentTenant`, `ClientIp`, `Email`, `PhoneNumber`, `Custom`. Custom algorithms and custom partition resolvers must be registered in `AbpOperationRateLimitingOptions`, otherwise create/update returns `LINGYUN.Abp.OperationRateLimiting.Management:010002`; duplicate names return `LINGYUN.Abp.OperationRateLimiting.Management:010001`.

## Merge rules

After a successful create/update/delete the app service publishes `OperationRateLimitingPolicyChangedEto` (policy and rule snapshot, empty for deletions) for cache refresh or audit subscribers, and immediately invalidates the dynamic policy cache of the current tenant.

Database policies are merged with the static ones by the base library (`DynamicDefinitionStrategy.Merge`): the database `DisplayName`, `ErrorCode`, `IsEnabled` override the static values, a non-empty rule list replaces the static rules, database-only policies apply as-is, and the cache is per tenant for 5 minutes (invalidated by the app service after writes).
