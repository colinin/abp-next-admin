# LINGYUN.Platform.EntityFrameworkCore

平台管理模块的EntityFrameworkCore实现，提供了数据访问和持久化功能。

## 功能特性

* 实现平台管理所有仓储接口
* 支持多数据库提供程序
* 实体关系映射配置
* 数据库上下文定义
* 支持查询优化和性能调优

## 模块引用

```csharp
[DependsOn(typeof(PlatformEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 仓储实现

* `EfCoreUserMenuRepository`: 用户菜单仓储实现
  * 支持获取用户启动菜单
  * 支持用户菜单列表查询
  * 支持用户菜单权限验证

* `EfCorePackageRepository`: 包管理仓储实现
  * 支持包版本查询
  * 支持包规格过滤
  * 支持包详情加载

* `EfCoreEnterpriseRepository`: 企业仓储实现
  * 支持租户关联查询
  * 支持企业列表分页

## 数据库上下文

* `IPlatformDbContext`: 平台管理数据库上下文接口
  * 定义所有实体的DbSet
  * 支持多租户数据隔离

## 配置项

```json
{
  "ConnectionStrings": {
    "Platform": "Server=localhost;Database=Platform;Trusted_Connection=True"
  }
}
```

## 更多

更多信息请参考 [Platform](../README.md)
