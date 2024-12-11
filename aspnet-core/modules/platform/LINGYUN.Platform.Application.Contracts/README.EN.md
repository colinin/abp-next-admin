# LINGYUN.Platform.Application.Contracts

The application service contract layer of the platform management module, defining application service interfaces, DTO objects, and permission definitions.

## Features

* Menu Management Interface
  * Menu CRUD operations
  * User menu management
  * Role menu management
  * Menu favorite functionality

* Package Management Interface
  * Package CRUD operations
  * Package version management
  * Package file upload and download

* Permission Definitions
  * Platform management permission group
  * Data dictionary permissions
  * Menu management permissions
  * Package management permissions

## Module Reference

```csharp
[DependsOn(typeof(PlatformApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Service Interfaces

* `IMenuAppService`: Menu management service interface
* `IUserFavoriteMenuAppService`: User favorite menu service interface
* `IPackageAppService`: Package management service interface

## Data Transfer Objects

* Menu Related DTOs
  * `MenuDto`: Menu DTO
  * `MenuCreateDto`: Create menu DTO
  * `MenuUpdateDto`: Update menu DTO
  * `MenuItemDto`: Menu item DTO
  * `UserFavoriteMenuDto`: User favorite menu DTO

* Package Management Related DTOs
  * `PackageDto`: Package DTO
  * `PackageCreateDto`: Create package DTO
  * `PackageUpdateDto`: Update package DTO
  * `PackageBlobDto`: Package file DTO

## Permission Definitions

```json
{
  "Platform": {
    "Default": "Platform Management",
    "DataDictionary": {
      "Default": "Data Dictionary Management",
      "Create": "Create",
      "Update": "Update",
      "Delete": "Delete"
    },
    "Menu": {
      "Default": "Menu Management",
      "Create": "Create",
      "Update": "Update",
      "Delete": "Delete"
    },
    "Package": {
      "Default": "Package Management",
      "Create": "Create",
      "Update": "Update",
      "Delete": "Delete"
    }
  }
}
```

## More

For more information, please refer to [Platform](../README.md)
