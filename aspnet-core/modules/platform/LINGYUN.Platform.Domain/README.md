# LINGYUN.Platform.Domain

平台管理模块的领域层，实现了平台管理所需的核心业务逻辑和领域模型。

## 功能特性

* 菜单管理
  * 支持多级菜单结构
  * 用户菜单定制
  * 角色菜单权限
  * 菜单标准化转换

* 布局管理
  * 布局视图实体
  * 布局数据关联
  * 多框架支持

* 数据字典
  * 数据字典管理
  * 数据字典项管理
  * 数据字典种子数据

* 包管理
  * 包版本控制
  * 包文件管理
  * Blob存储集成
  * 包过滤规范

* 企业门户
  * 企业信息管理
  * 企业数据存储

## 模块引用

```csharp
[DependsOn(typeof(PlatformDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 领域服务

* `DataDictionaryDataSeeder`: 数据字典种子数据服务
* `PackageBlobManager`: 包文件管理服务
* `DefaultStandardMenuConverter`: 标准菜单转换服务

## 仓储接口

* `IMenuRepository`: 菜单仓储接口
* `IUserMenuRepository`: 用户菜单仓储接口
* `IRoleMenuRepository`: 角色菜单仓储接口
* `ILayoutRepository`: 布局仓储接口
* `IEnterpriseRepository`: 企业仓储接口

## 实体

* `Menu`: 菜单实体
* `UserMenu`: 用户菜单实体
* `RoleMenu`: 角色菜单实体
* `Layout`: 布局实体
* `Package`: 包实体
* `Data`: 数据字典实体
* `DataItem`: 数据字典项实体

## 更多

更多信息请参考 [Platform](../README.md)
