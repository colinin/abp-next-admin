# LINGYUN.Abp.Saas.EntityFrameworkCore

SaaS EntityFrameworkCore module, implementing the data access layer for tenants and editions.

## Features

* Entity Mapping Configuration
  * Tenant entity mapping
  * Edition entity mapping
  * Tenant connection string entity mapping

* Repository Implementation
  * EfCoreTenantRepository: Tenant repository implementation
  * EfCoreEditionRepository: Edition repository implementation

* Database Tables
  * AbpEditions: Edition table
  * AbpTenants: Tenant table
  * AbpTenantConnectionStrings: Tenant connection string table

## Configuration

You can modify the database table prefix and Schema through configuration:

```json
{
  "AbpSaas": {
    "EntityFrameworkCore": {
      "TablePrefix": "Abp",  // Database table prefix
      "Schema": null         // Database Schema
    }
  }
}
```

## More

[简体中文](README.md)
