# LINGYUN.Abp.PermissionManagement.HttpApi

权限管理HTTP API模块，提供权限管理的RESTful API接口。

## 功能特性

* 权限组定义API
  * 提供权限组的CRUD操作接口
  * 支持权限组分页查询
* 权限定义API
  * 提供权限的CRUD操作接口
  * 支持权限分页查询
* 统一的API基类
  * PermissionManagementControllerBase - 权限管理控制器基类
  * 标准化的API响应格式

## 模块引用

```csharp
[DependsOn(
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## API接口

1. 权限组定义
```
GET    /api/permission-management/groups
POST   /api/permission-management/groups
PUT    /api/permission-management/groups/{name}
DELETE /api/permission-management/groups/{name}
```

2. 权限定义
```
GET    /api/permission-management/permissions
POST   /api/permission-management/permissions
PUT    /api/permission-management/permissions/{name}
DELETE /api/permission-management/permissions/{name}
```

## 基本用法

1. 权限组定义管理
```http
### 创建权限组
POST /api/permission-management/groups
{
    "name": "MyPermissionGroup",
    "displayName": "My Permission Group"
}

### 更新权限组
PUT /api/permission-management/groups/MyPermissionGroup
{
    "displayName": "Updated Permission Group"
}
```

2. 权限定义管理
```http
### 创建权限
POST /api/permission-management/permissions
{
    "groupName": "MyPermissionGroup",
    "name": "MyPermission",
    "displayName": "My Permission",
    "providers": ["Role", "User"]
}

### 更新权限
PUT /api/permission-management/permissions/MyPermission
{
    "displayName": "Updated Permission"
}
```

## 另请参阅

* [LINGYUN.Abp.PermissionManagement.Application](../LINGYUN.Abp.PermissionManagement.Application/README.md)
* [LINGYUN.Abp.PermissionManagement.Application.Contracts](../LINGYUN.Abp.PermissionManagement.Application.Contracts/README.md)
* [LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits](../LINGYUN.Abp.PermissionManagement.Domain.OrganizationUnits/README.md)
