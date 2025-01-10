# LINGYUN.Platform.Domain

The domain layer of the platform management module, implementing core business logic and domain models required for platform management.

## Features

* Menu Management
  * Support for multi-level menu structure
  * User menu customization
  * Role menu permissions
  * Menu standardization conversion

* Layout Management
  * Layout view entities
  * Layout data association
  * Multi-framework support

* Data Dictionary
  * Data dictionary management
  * Data dictionary item management
  * Data dictionary seed data

* Package Management
  * Package version control
  * Package file management
  * Blob storage integration
  * Package filtering specification

* Enterprise Portal
  * Enterprise information management
  * Enterprise data storage

## Module Reference

```csharp
[DependsOn(typeof(PlatformDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Domain Services

* `DataDictionaryDataSeeder`: Data dictionary seed data service
* `PackageBlobManager`: Package file management service
* `DefaultStandardMenuConverter`: Standard menu conversion service

## Repository Interfaces

* `IMenuRepository`: Menu repository interface
* `IUserMenuRepository`: User menu repository interface
* `IRoleMenuRepository`: Role menu repository interface
* `ILayoutRepository`: Layout repository interface
* `IEnterpriseRepository`: Enterprise repository interface

## Entities

* `Menu`: Menu entity
* `UserMenu`: User menu entity
* `RoleMenu`: Role menu entity
* `Layout`: Layout entity
* `Package`: Package entity
* `Data`: Data dictionary entity
* `DataItem`: Data dictionary item entity

## More

For more information, please refer to [Platform](../README.md)
