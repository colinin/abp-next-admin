# LINGYUN.Platform.HttpApi

The HTTP API layer of the platform management module, providing RESTful style API interfaces.

## Features

* Menu Management API
  * Get current user menu
  * Get user menu list
  * Get role menu list
  * Menu CRUD operations
  * User favorite menu management

* Layout Management API
  * Layout CRUD operations
  * Get all layouts list

* Data Dictionary API
  * Data dictionary CRUD operations
  * Data dictionary item management

* Package Management API
  * Package CRUD operations
  * Package file upload and download
  * Get latest version package

## Module Reference

```csharp
[DependsOn(typeof(PlatformHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Controllers

* `MenuController`: Menu management controller
  * `GET /api/platform/menus/by-current-user`: Get current user menu
  * `GET /api/platform/menus/by-user`: Get specified user menu
  * `GET /api/platform/menus/by-role`: Get role menu
  * `POST /api/platform/menus`: Create menu
  * `PUT /api/platform/menus/{id}`: Update menu
  * `DELETE /api/platform/menus/{id}`: Delete menu

* `UserFavoriteMenuController`: User favorite menu controller
  * `GET /api/platform/menus/favorites/my-favorite-menus`: Get my favorite menus
  * `POST /api/platform/menus/favorites/my-favorite-menus`: Create favorite menu
  * `PUT /api/platform/menus/favorites/my-favorite-menus/{MenuId}`: Update favorite menu
  * `DELETE /api/platform/menus/favorites/my-favorite-menus`: Delete favorite menu

* `PackageController`: Package management controller
  * `GET /api/platform/packages/{Name}/latest`: Get latest version package
  * `POST /api/platform/packages/{id}/blob`: Upload package file
  * `GET /api/platform/packages/{id}/blob/{Name}`: Download package file
  * `DELETE /api/platform/packages/{id}/blob/{Name}`: Delete package file

## Configuration

```json
{
  "App": {
    "CorsOrigins": "https://*.YourDomain.com,http://localhost:4200"
  }
}
```

## More

For more information, please refer to [Platform](../README.md)
