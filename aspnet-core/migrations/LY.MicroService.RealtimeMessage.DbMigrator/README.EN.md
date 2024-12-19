# LY.MicroService.RealtimeMessage.DbMigrator

Real-time Message Database Migration Console Application, used for executing database migrations and initializing seed data for the real-time message system.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Initialize necessary system seed data
* Support command line parameter configuration
* Integrated Autofac dependency injection container
* Inherit all migration features from real-time message system
* Support SignalR message management
* Support message group management
* Support message subscription management
* Support message history

## Module Dependencies

```csharp
[DependsOn(
    typeof(RealtimeMessageMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "RealtimeMessageDbMigrator": "Your database connection string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure RealtimeMessageDbMigrator connection string in appsettings.json

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

* Default message group configurations
* Basic message subscription configurations
* System message templates
* Default permission configurations

## More Information

* [ABP Documentation](https://docs.abp.io)
* [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr/introduction)
