# LINGYUN.Platform.Application.Contracts

平台管理模块的应用服务契约层，定义了应用服务接口、DTO对象和权限定义。

## 功能特性

* 菜单管理接口
  * 菜单CRUD操作
  * 用户菜单管理
  * 角色菜单管理
  * 菜单收藏功能

* 包管理接口
  * 包CRUD操作
  * 包版本管理
  * 包文件上传下载

* 权限定义
  * 平台管理权限组
  * 数据字典权限
  * 菜单管理权限
  * 包管理权限

## 模块引用

```csharp
[DependsOn(typeof(PlatformApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务接口

* `IMenuAppService`: 菜单管理服务接口
* `IUserFavoriteMenuAppService`: 用户收藏菜单服务接口
* `IPackageAppService`: 包管理服务接口

## 数据传输对象

* 菜单相关DTO
  * `MenuDto`: 菜单DTO
  * `MenuCreateDto`: 创建菜单DTO
  * `MenuUpdateDto`: 更新菜单DTO
  * `MenuItemDto`: 菜单项DTO
  * `UserFavoriteMenuDto`: 用户收藏菜单DTO

* 包管理相关DTO
  * `PackageDto`: 包DTO
  * `PackageCreateDto`: 创建包DTO
  * `PackageUpdateDto`: 更新包DTO
  * `PackageBlobDto`: 包文件DTO

## 权限定义

```json
{
  "Platform": {
    "Default": "平台管理",
    "DataDictionary": {
      "Default": "数据字典管理",
      "Create": "创建",
      "Update": "更新",
      "Delete": "删除"
    },
    "Menu": {
      "Default": "菜单管理",
      "Create": "创建",
      "Update": "更新",
      "Delete": "删除"
    },
    "Package": {
      "Default": "包管理",
      "Create": "创建",
      "Update": "更新",
      "Delete": "删除"
    }
  }
}
```

## 更多

更多信息请参考 [Platform](../README.md)
