# LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql

MySql database provider module for Elsa workflow

## Features

* Provides MySql database support for Elsa workflow
* Depends on `AbpElsaEntityFrameworkCoreModule` and `AbpEntityFrameworkCoreMySQLModule`
* Pre-configures the following features:
  * Persistence storage
  * Webhooks storage
  * Workflow settings storage

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreMySqlModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## Configuration Options

```json
{
    "Elsa": {
        "Persistence": true,     // Enable persistence storage
        "Webhooks": true,       // Enable webhooks storage
        "WorkflowSettings": true // Enable workflow settings storage
    }
}
```

[中文文档](./README.md)
