# LINGYUN.Platform.HttpApi

平台管理模块的HTTP API层，提供了RESTful风格的API接口。

## 功能特性

* 菜单管理API
  * 获取当前用户菜单
  * 获取用户菜单列表
  * 获取角色菜单列表
  * 菜单CRUD操作
  * 用户收藏菜单管理

* 布局管理API
  * 布局CRUD操作
  * 获取所有布局列表

* 数据字典API
  * 数据字典CRUD操作
  * 数据字典项管理

* 包管理API
  * 包CRUD操作
  * 包文件上传下载
  * 获取最新版本包

## 模块引用

```csharp
[DependsOn(typeof(PlatformHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API控制器

* `MenuController`: 菜单管理控制器
  * `GET /api/platform/menus/by-current-user`: 获取当前用户菜单
  * `GET /api/platform/menus/by-user`: 获取指定用户菜单
  * `GET /api/platform/menus/by-role`: 获取角色菜单
  * `POST /api/platform/menus`: 创建菜单
  * `PUT /api/platform/menus/{id}`: 更新菜单
  * `DELETE /api/platform/menus/{id}`: 删除菜单

* `UserFavoriteMenuController`: 用户收藏菜单控制器
  * `GET /api/platform/menus/favorites/my-favorite-menus`: 获取我的收藏菜单
  * `POST /api/platform/menus/favorites/my-favorite-menus`: 创建收藏菜单
  * `PUT /api/platform/menus/favorites/my-favorite-menus/{MenuId}`: 更新收藏菜单
  * `DELETE /api/platform/menus/favorites/my-favorite-menus`: 删除收藏菜单

* `PackageController`: 包管理控制器
  * `GET /api/platform/packages/{Name}/latest`: 获取最新版本包
  * `POST /api/platform/packages/{id}/blob`: 上传包文件
  * `GET /api/platform/packages/{id}/blob/{Name}`: 下载包文件
  * `DELETE /api/platform/packages/{id}/blob/{Name}`: 删除包文件

## 配置项

```json
{
  "App": {
    "CorsOrigins": "https://*.YourDomain.com,http://localhost:4200"
  }
}
```

## 更多

更多信息请参考 [Platform](../README.md)
