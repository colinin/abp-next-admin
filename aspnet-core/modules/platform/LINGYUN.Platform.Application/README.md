# LINGYUN.Platform.Application

平台管理模块的应用服务实现层，实现了应用服务接口定义的所有功能。

## 功能特性

* 用户收藏菜单服务
  * 创建收藏菜单
  * 更新收藏菜单
  * 删除收藏菜单
  * 查询收藏菜单列表
  * 管理其他用户的收藏菜单

* 对象映射配置
  * 实体到DTO的自动映射
  * 支持自定义映射规则
  * 支持额外属性映射

* 权限验证
  * 基于策略的权限验证
  * 集成ABP授权系统
  * 细粒度的权限控制

## 模块引用

```csharp
[DependsOn(typeof(PlatformApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务实现

* `UserFavoriteMenuAppService`: 用户收藏菜单服务实现
  * 支持用户自定义菜单图标
  * 支持用户自定义菜单颜色
  * 支持用户自定义菜单别名
  * 支持多框架菜单管理

## 对象映射

```csharp
public class PlatformApplicationMappingProfile : Profile
{
    public PlatformApplicationMappingProfile()
    {
        CreateMap<PackageBlob, PackageBlobDto>();
        CreateMap<Package, PackageDto>();
        CreateMap<DataItem, DataItemDto>();
        CreateMap<Data, DataDto>();
        CreateMap<Menu, MenuDto>();
        CreateMap<Layout, LayoutDto>();
        CreateMap<UserFavoriteMenu, UserFavoriteMenuDto>();
    }
}
```

## 基础服务

* `PlatformApplicationServiceBase`: 平台管理应用服务基类
  * 提供通用功能和帮助方法
  * 统一异常处理
  * 统一权限验证

## 更多

更多信息请参考 [Platform](../README.md)
