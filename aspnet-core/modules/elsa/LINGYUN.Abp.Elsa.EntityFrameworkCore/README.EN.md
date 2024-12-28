# LINGYUN.Abp.Elsa.EntityFrameworkCore

EntityFrameworkCore integration module for Elsa workflow

## Features

* Provides EntityFrameworkCore integration for Elsa workflow
* Depends on `AbpElsaModule` and `AbpEntityFrameworkCoreModule`

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## Database Providers

This module is database provider agnostic. You need to choose one of the following modules based on your actual database:

* [SqlServer](../LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer/README.md)
* [PostgreSql](../LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql/README.md)
* [MySql](../LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql/README.md)

[中文文档](./README.md)
