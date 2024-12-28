# LINGYUN.Abp.PermissionManagement.HttpApi

Permission management HTTP API module, providing RESTful API interfaces for permission management.

## Features

* Permission Group Definition API
  * Provides CRUD operation interfaces for permission groups
  * Supports pagination query for permission groups
* Permission Definition API
  * Provides CRUD operation interfaces for permissions
  * Supports pagination query for permissions
* Unified API Base Classes
  * PermissionManagementControllerBase - Permission management controller base class
  * Standardized API response format

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## API Endpoints

1. Permission Group Definition
```
GET    /api/permission-management/groups
POST   /api/permission-management/groups
PUT    /api/permission-management/groups/{name}
DELETE /api/permission-management/groups/{name}
```

2. Permission Definition
```
GET    /api/permission-management/permissions
POST   /api/permission-management/permissions
PUT    /api/permission-management/permissions/{name}
DELETE /api/permission-management/permissions/{name}
```

## Basic Usage

1. Permission Group Definition Management
```http
### Create permission group
POST /api/permission-management/groups
{
    "name": "MyPermissionGroup",
    "displayName": "My Permission Group"
}

### Update permission group
PUT /api/permission-management/groups/MyPermissionGroup
{
    "displayName": "Updated Permission Group"
}
```

2. Permission Definition Management
```http
### Create permission
POST /api/permission-management/permissions
{
    "groupName": "MyPermissionGroup",
    "name": "MyPermission",
    "displayName": "My Permission",
    "providers": ["Role", "User"]
}

### Update permission
PUT /api/permission-management/permissions/MyPermission
{
    "displayName": "Updated Permission"
}
```

## See Also

* [LINGYUN.Abp.PermissionManagement.Application](../LINGYUN.Abp.PermissionManagement.Application/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.Application.Contracts](../LINGYUN.Abp.PermissionManagement.Application.Contracts/README.EN.md)
* [LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits](../LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits/README.EN.md)
