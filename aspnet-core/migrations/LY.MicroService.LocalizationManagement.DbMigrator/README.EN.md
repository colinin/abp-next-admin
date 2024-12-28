# LY.MicroService.LocalizationManagement.DbMigrator

Localization Management Database Migration Console Application, used for executing database migrations and initializing seed data for the localization management system.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Initialize necessary system seed data
* Support command line parameter configuration
* Integrated Autofac dependency injection container
* Inherit all migration features from localization management system
* Support multi-language resource management
* Support dynamic language text management
* Support language management

## Module Dependencies

```csharp
[DependsOn(
    typeof(LocalizationManagementMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "LocalizationManagementDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure LocalizationManagementDbMigrator connection string in appsettings.json

2. Run Migration Program
   ```bash
   dotnet run
   ```

## Command Line Arguments

* --database-provider
  * Specify database provider (default: MySQL)
* --connection-string
  * Specify database connection string
* --skip-db-migrations
  * Skip database migrations
* --skip-seed-data
  * Skip seed data initialization

## Seed Data

* Default language configurations
* Basic localization resources
* System text resources
* Default permission configurations

## More Information

* [ABP Documentation](https://docs.abp.io)
* [ABP Localization Documentation](https://docs.abp.io/en/abp/latest/Localization)
