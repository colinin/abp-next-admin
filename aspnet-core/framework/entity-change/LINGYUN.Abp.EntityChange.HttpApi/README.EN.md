# LINGYUN.Abp.EntityChange.HttpApi

HTTP API module for entity change tracking and restoration.

## Features

* Provides HTTP API for entity change queries
* Provides HTTP API for entity restoration
* Supports multilingual localization

## Basic Usage

```csharp
[DependsOn(typeof(AbpEntityChangeHttpApiModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ...
    }
}
```

## API Endpoints

* `EntityChangeController`: Entity change controller
  * GET `/api/entity-changes`: Get list of entity change history records

* `EntityRestoreController`: Entity restoration controller
  * PUT `/api/entity-restore`: Restore a single entity to specified version
  * PUT `/api/{EntityId}/entity-restore`: Restore entity with specified ID
  * PUT `/api/{EntityId}/v/{EntityChangeId}/entity-restore`: Restore entity with specified ID to specified version
  * PUT `/api/entites-restore`: Batch restore entities to specified versions

## Localization

The module uses `AbpEntityChangeResource` as localization resource, supporting multilingual display.

[简体中文](./README.md)
