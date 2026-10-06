# LY.MicroService.Platform.DbMigrator

Platform Management Database Migration Console Application, used for executing database migrations and initializing seed data for the platform management system.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Initialize necessary system seed data
* Support command line parameter configuration
* Integrated Autofac dependency injection container
* Inherit all migration features from platform management system
* Support data dictionary management
* Support organization management
* Support menu management
* Support version management
* Support Vue Vben Admin UI navigation

## Module Dependencies

```csharp
[DependsOn(
    typeof(PlatformMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "PlatformDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure PlatformDbMigrator connection string in appsettings.json

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

* Default data dictionaries
* Basic organizations
* System menus
* Default version configurations
* UI navigation configurations

## More Information

* [ABP Documentation](https://docs.abp.io)
* [Vue Vben Admin Documentation](https://doc.vvbin.cn/)
