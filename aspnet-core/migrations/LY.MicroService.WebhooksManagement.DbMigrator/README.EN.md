# LY.MicroService.WebhooksManagement.DbMigrator

Webhooks Management Database Migration Console Application, used for executing database migrations and initializing seed data for the webhooks management system.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Initialize necessary system seed data
* Support command line parameter configuration
* Integrated Autofac dependency injection container
* Inherit all migration features from webhooks management system

## Module Dependencies

```csharp
[DependsOn(
    typeof(WebhooksManagementMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "WebhooksManagementDbMigrator": "Your database connection string"
  },
  "IdentityServer": {
    "Clients": {
      "WebhooksManagement_App": {
        "ClientId": "WebhooksManagement_App"
      }
    }
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure WebhooksManagementDbMigrator connection string in appsettings.json

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

## More Information

* [ABP Documentation](https://docs.abp.io)
* [EF Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
