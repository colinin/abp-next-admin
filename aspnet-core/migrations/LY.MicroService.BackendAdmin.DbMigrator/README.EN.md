# LY.MicroService.BackendAdmin.DbMigrator

Backend Administration System Database Migration Console Application, used for executing database migrations and initializing seed data for the backend administration system.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Initialize necessary system seed data
* Support command line parameter configuration
* Integrated Autofac dependency injection container
* Integrated Feature Management
* Integrated Setting Management
* Integrated Permission Management
* Integrated Localization Management
* Integrated Cache Management
* Integrated Auditing
* Integrated Text Templating
* Integrated Identity Authentication
* Integrated IdentityServer
* Integrated OpenIddict
* Integrated Platform Management
* Integrated Object Storage Management
* Integrated Notification System
* Integrated Message Service
* Integrated Task Management
* Integrated Webhooks Management

## Module Dependencies

```csharp
[DependsOn(
    typeof(BackendAdminMigrationsEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpLocalizationManagementApplicationContractsModule),
    typeof(AbpCachingManagementApplicationContractsModule),
    typeof(AbpAuditingApplicationContractsModule),
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpIdentityServerApplicationContractsModule),
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(PlatformApplicationContractModule),
    typeof(AbpOssManagementApplicationContractsModule),
    typeof(AbpNotificationsApplicationContractsModule),
    typeof(AbpMessageServiceApplicationContractsModule),
    typeof(TaskManagementApplicationContractsModule),
    typeof(WebhooksManagementApplicationContractsModule),
    typeof(AbpAutofacModule)
)]
```

## Configuration

```json
{
  "ConnectionStrings": {
    "BackendAdminDbMigrator": "Your database connection string"
  },
  "IdentityServer": {
    "Clients": {
      "BackendAdmin_App": {
        "ClientId": "BackendAdmin_App"
      }
    }
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure BackendAdminDbMigrator connection string in appsettings.json

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
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io)
* [OpenIddict Documentation](https://documentation.openiddict.com)
