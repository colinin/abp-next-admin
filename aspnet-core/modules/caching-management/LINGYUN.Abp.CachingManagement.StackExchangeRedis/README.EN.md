# LINGYUN.Abp.CachingManagement.StackExchangeRedis

Cache management implementation module based on StackExchange.Redis.

## Features

* Implements the `ICacheManager` interface, providing Redis-based cache management features:
  * Get cache key list (supports prefix matching and filtering)
  * Get cache value
  * Set cache value (supports absolute and sliding expiration)
  * Refresh cache
  * Delete cache

## Installation

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## Configuration and Usage

1. First, configure the Redis connection string in `appsettings.json`:

```json
{
  "Redis": {
    "Configuration": "127.0.0.1:6379"
  }
}
```

2. Add the following code in the `ConfigureServices` method of your module:

```csharp
Configure<RedisCacheOptions>(options =>
{
    // Optional: Configure Redis instance name prefix
    options.InstanceName = "MyApp:";
});

Configure<AbpDistributedCacheOptions>(options =>
{
    // Optional: Configure cache key prefix
    options.KeyPrefix = "MyApp:";
});
```

## Multi-tenancy Support

The module has built-in multi-tenancy support, automatically isolating cache data based on the current tenant ID:
* Key format with tenant: `{InstanceName}t:{TenantId}:{KeyPrefix}:{Key}`
* Key format without tenant: `{InstanceName}c:{KeyPrefix}:{Key}`

## More

For more information, please refer to the following resources:

* [Application Service Implementation](../LINGYUN.Abp.CachingManagement.Application/README.EN.md)
* [Application Service Contracts](../LINGYUN.Abp.CachingManagement.Application.Contracts/README.EN.md)
* [HTTP API](../LINGYUN.Abp.CachingManagement.HttpApi/README.EN.md)
* [Domain Layer](../LINGYUN.Abp.CachingManagement.Domain/README.EN.md)
