# LINGYUN.Abp.LocalizationManagement.Domain

The domain layer module for localization management, implementing dynamic localization resource storage and management functionality.

## Features

* Implements `ILocalizationStore` interface for localization resource storage and retrieval
* Supports language management (CRUD operations)
* Supports resource management (CRUD operations)
* Supports text management (CRUD operations)
* Supports in-memory caching of localization resources
* Supports distributed cache synchronization

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLocalizationManagementDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "LocalizationManagement": {
    "LocalizationCacheStampTimeOut": "00:02:00",  // Localization cache timestamp timeout, default 2 minutes
    "LocalizationCacheStampExpiration": "00:30:00" // Localization cache expiration time, default 30 minutes
  }
}
```

## Domain Services

* `LanguageManager`: Language management service, providing language creation, update, deletion, etc.
* `ResourceManager`: Resource management service, providing resource creation, update, deletion, etc.
* `TextManager`: Text management service, providing text creation, update, deletion, etc.
* `LocalizationStore`: Localization storage service, implementing the `ILocalizationStore` interface
* `LocalizationStoreInMemoryCache`: In-memory cache service for localization resources

## More Information

* [中文文档](./README.md)
