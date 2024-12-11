# LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer

SqlServer database provider module for Elsa workflow

## Features

* Provides SqlServer database support for Elsa workflow
* Depends on `AbpElsaEntityFrameworkCoreModule` and `AbpEntityFrameworkCoreSqlServerModule`
* Pre-configures the following features:
  * Persistence storage
  * Webhooks storage
  * Workflow settings storage

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreSqlServerModule)
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
