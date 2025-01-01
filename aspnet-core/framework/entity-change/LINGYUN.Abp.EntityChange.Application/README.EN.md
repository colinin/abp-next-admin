# LINGYUN.Abp.EntityChange.Application

Application layer implementation module for entity change tracking and restoration.

## Features

* Implements entity change query service
* Implements entity restoration service
* Provides auto mapping configuration for entity changes

## Basic Usage

```csharp
[DependsOn(typeof(AbpEntityChangeApplicationModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ...
    }
}
```

## Service Implementation

* `EntityRestoreAppService<TEntity, TKey>`: Entity restoration service implementation
  * `RestoreEntityAsync`: Restore a single entity to specified version through audit log
  * `RestoreEntitesAsync`: Batch restore entities to specified versions through audit log
  * Supports restoration permission policy configuration via `RestorePolicy`

## Object Mapping

The module uses AutoMapper to implement automatic mapping for the following objects:
* `EntityPropertyChange` -> `EntityPropertyChangeDto`
* `EntityChange` -> `EntityChangeDto`

[简体中文](./README.md)
