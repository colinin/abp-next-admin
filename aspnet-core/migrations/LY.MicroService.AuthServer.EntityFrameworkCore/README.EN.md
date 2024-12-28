# LY.MicroService.AuthServer.EntityFrameworkCore

Authentication server database migration module, providing database migration functionality required by the authentication server.

[简体中文](./README.md)

## Features

* Integrated OpenIddict authentication framework database migrations
* Integrated ABP Identity module database migrations
* Integrated ABP permission management module database migrations
* Integrated ABP settings management module database migrations
* Integrated ABP feature management module database migrations
* Integrated ABP SaaS multi-tenant module database migrations
* Integrated text templating module database migrations
* Support for MySQL database

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule)
)]
public class YourModule : AbpModule
{
    // other
}
```

## Configuration

```json
{
  "ConnectionStrings": {
    "AuthServerDbMigrator": "your-database-connection-string"
  }
}
```

## Basic Usage

1. Configure Database Connection String
   * Configure AuthServerDbMigrator connection string in appsettings.json

2. Add Database Context
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       context.Services.AddAbpDbContext<AuthServerMigrationsDbContext>();

       Configure<AbpDbContextOptions>(options =>
       {
           options.UseMySQL();
       });
   }
   ```

3. Execute Database Migration
   * Use EF Core CLI tools to execute migrations
   ```bash
   dotnet ef database update
   ```

## Notes

* Ensure the database connection string includes the correct permissions to create and modify database tables
* It's recommended to backup the database before executing migrations
* Migration scripts will automatically handle dependencies between modules
