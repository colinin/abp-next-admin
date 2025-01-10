# LINGYUN.Abp.CachingManagement.Domain

Domain layer implementation of the cache management module.

## Core Interfaces

### ICacheManager

Cache manager interface, defining core cache management functionality:

* `GetKeysAsync`: Get cache key list
* `GetValueAsync`: Get cache value
* `SetAsync`: Set cache value
* `RefreshAsync`: Refresh cache
* `RemoveAsync`: Remove cache

## Domain Services

### CacheManager

Abstract base class for cache manager, providing basic cache management implementation. Specific cache providers (like Redis) need to inherit this class and implement the corresponding methods.

## Installation

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## Extension Development

To support a new cache provider, you need to:

1. Create a new project, inherit from `CacheManager` class
2. Implement all abstract methods
3. Register as implementation of `ICacheManager` (using `[Dependency(ReplaceServices = true)]`)

## More

For more information, please refer to the following resources:

* [Application Service Implementation](../LINGYUN.Abp.CachingManagement.Application/README.EN.md)
* [Application Service Contracts](../LINGYUN.Abp.CachingManagement.Application.Contracts/README.EN.md)
* [HTTP API](../LINGYUN.Abp.CachingManagement.HttpApi/README.EN.md)
* [Redis Implementation](../LINGYUN.Abp.CachingManagement.StackExchangeRedis/README.EN.md)
