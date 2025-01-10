# LY.MicroService.IdentityServer.DbMigrator

Identity Server Database Migration Console Application, used for executing database migrations and initializing seed data for the identity server.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Initialize necessary system seed data
* Support command line parameter configuration
* Integrated Autofac dependency injection container
* Inherit all migration features from identity server
* Support IdentityServer4 configuration
* Support OAuth 2.0 and OpenID Connect protocols
* Support user and role management
* Support client and API resource management

## Module Dependencies

```csharp
[DependsOn(
    typeof(IdentityServerMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "IdentityServerDbMigrator": "Your database connection string"
  },
  "IdentityServer": {
    "Clients": {
      "IdentityServer_App": {
        "ClientId": "IdentityServer_App"
      }
    }
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure IdentityServerDbMigrator connection string in appsettings.json

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

* Default users and roles
* Standard identity resources
* API resources and scopes
* Default client configurations
* Basic permission configurations

## More Information

* [ABP Documentation](https://docs.abp.io)
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io)
* [OAuth 2.0 Specification](https://oauth.net/2/)
* [OpenID Connect Specification](https://openid.net/connect/)
