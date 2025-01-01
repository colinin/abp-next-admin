# LINGYUN.Platform.EntityFrameworkCore

The EntityFrameworkCore implementation of the platform management module, providing data access and persistence functionality.

## Features

* Implementation of all platform management repository interfaces
* Support for multiple database providers
* Entity relationship mapping configuration
* Database context definition
* Support for query optimization and performance tuning

## Module Reference

```csharp
[DependsOn(typeof(PlatformEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Repository Implementations

* `EfCoreUserMenuRepository`: User menu repository implementation
  * Support for getting user startup menu
  * Support for user menu list query
  * Support for user menu permission validation

* `EfCorePackageRepository`: Package management repository implementation
  * Support for package version query
  * Support for package specification filtering
  * Support for package details loading

* `EfCoreEnterpriseRepository`: Enterprise repository implementation
  * Support for tenant association query
  * Support for enterprise list pagination

## Database Context

* `IPlatformDbContext`: Platform management database context interface
  * Define DbSet for all entities
  * Support for multi-tenant data isolation

## Configuration

```json
{
  "ConnectionStrings": {
    "Platform": "Server=localhost;Database=Platform;Trusted_Connection=True"
  }
}
```

## More

For more information, please refer to [Platform](../README.md)
