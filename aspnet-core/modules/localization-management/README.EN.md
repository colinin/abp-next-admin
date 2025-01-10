# Localization Management

Localization document management module. Due to long project paths not being supported in Windows systems, the project directory uses the abbreviation 'lt'.

## Features

* Support dynamic management of localization resources
* Support language management (CRUD operations)
* Support resource management (CRUD operations)
* Support text management (CRUD operations)
* Support in-memory caching of localization resources
* Support distributed cache synchronization
* Provide standard RESTful API interfaces
* Seamless integration with ABP framework

## Module Description

### Basic Modules

* [LINGYUN.Abp.Localization.Persistence](../localization/LINGYUN.Abp.Localization.Persistence) - Localization persistence module, implements IStaticLocalizationSaver interface to persist local static resources to storage facilities
* [LINGYUN.Abp.LocalizationManagement.Domain.Shared](./LINGYUN.Abp.LocalizationManagement.Domain.Shared) - Domain layer shared module, defines error codes, localization, and module settings
* [LINGYUN.Abp.LocalizationManagement.Domain](./LINGYUN.Abp.LocalizationManagement.Domain) - Domain layer module, implements ILocalizationStore interface
* [LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore](./LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore) - Data access layer module, integrates EFCore
* [LINGYUN.Abp.LocalizationManagement.Application.Contracts](./LINGYUN.Abp.LocalizationManagement.Application.Contracts) - Application service layer shared module, defines external interfaces, permissions, and functionality restriction policies for managing localization objects
* [LINGYUN.Abp.LocalizationManagement.Application](./LINGYUN.Abp.LocalizationManagement.Application) - Application service layer implementation, implements localization object management interfaces
* [LINGYUN.Abp.LocalizationManagement.HttpApi](./LINGYUN.Abp.LocalizationManagement.HttpApi) - RestApi implementation, implements independent external RestApi interfaces

### Advanced Modules

No advanced modules at present.

### Permission Definitions

* LocalizationManagement.Resource - Authorizes access to resources
* LocalizationManagement.Resource.Create - Authorizes resource creation
* LocalizationManagement.Resource.Update - Authorizes resource modification
* LocalizationManagement.Resource.Delete - Authorizes resource deletion
* LocalizationManagement.Language - Authorizes access to languages
* LocalizationManagement.Language.Create - Authorizes language creation
* LocalizationManagement.Language.Update - Authorizes language modification
* LocalizationManagement.Language.Delete - Authorizes language deletion
* LocalizationManagement.Text - Authorizes access to texts
* LocalizationManagement.Text.Create - Authorizes text creation
* LocalizationManagement.Text.Update - Authorizes text modification
* LocalizationManagement.Text.Delete - Authorizes text deletion

### Configuration

```json
{
  "LocalizationManagement": {
    "LocalizationCacheStampTimeOut": "00:02:00",  // Localization cache timestamp timeout, default 2 minutes
    "LocalizationCacheStampExpiration": "00:30:00" // Localization cache expiration time, default 30 minutes
  }
}
```

### Database Tables

The module uses the following database tables to store localization data:

* Languages - Language table
* Resources - Resource table
* Texts - Text table

Table prefix and schema can be configured through `LocalizationModelBuilderConfigurationOptions`:

```csharp
public class LocalizationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public LocalizationModelBuilderConfigurationOptions(
       string tablePrefix = "",
       string schema = null)
       : base(tablePrefix, schema)
    {
    }
}
```

### API Endpoints

The module provides the following REST API endpoints:

* `/api/localization-management/languages` - Language management related APIs
* `/api/localization-management/resources` - Resource management related APIs
* `/api/localization-management/texts` - Text management related APIs

## Error Codes

* Localization:001100 - Language {CultureName} already exists
* Localization:001400 - Language name {CultureName} not found or built-in language operation not allowed
* Localization:002100 - Resource {Name} already exists
* Localization:002400 - Resource name {Name} not found or built-in resource operation not allowed

## More Information

* [中文文档](./README.md)

## Change Log

### 2024.12
* Improved module documentation
* Added English documentation support
