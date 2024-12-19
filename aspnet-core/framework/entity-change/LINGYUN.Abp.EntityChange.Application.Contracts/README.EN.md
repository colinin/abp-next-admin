# LINGYUN.Abp.EntityChange.Application.Contracts

Application contracts module for entity change tracking and restoration.

## Features

* Provides entity change query interface
* Provides entity restoration interface
* Provides entity change related DTOs
* Provides multilingual localization resources

## Basic Usage

```csharp
[DependsOn(typeof(AbpEntityChangeApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ...
    }
}
```

## API

* `IEntityChangeAppService`: Entity change query service interface
  * `GetListAsync`: Get list of entity change history records

* `IEntityRestoreAppService`: Entity restoration service interface
  * `RestoreEntityAsync`: Restore a single entity to specified version
  * `RestoreEntitesAsync`: Batch restore entities to specified versions

## Localization

The module provides localization resources in the following languages:
* en
* zh-Hans

Localization resources are located in the `/LINGYUN/Abp/EntityChange/Localization/Resources` directory.

[简体中文](./README.md)
