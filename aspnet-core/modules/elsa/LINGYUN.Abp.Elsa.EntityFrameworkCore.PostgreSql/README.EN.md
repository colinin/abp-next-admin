# LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql

PostgreSql database provider module for Elsa workflow

## Features

* Provides PostgreSql database support for Elsa workflow
* Depends on `AbpElsaEntityFrameworkCoreModule` and `AbpEntityFrameworkCorePostgreSqlModule`
* Pre-configures the following features:
  * Persistence storage
  * Webhooks storage
  * Workflow settings storage

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaEntityFrameworkCorePostgreSqlModule)
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
