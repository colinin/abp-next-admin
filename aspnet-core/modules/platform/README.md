# LINGYUN.Platform

平台管理模块，提供了一套完整的平台管理功能，包括菜单管理、布局管理、数据字典、包管理等功能。

## 功能特性

* 菜单管理
  * 多级菜单结构
  * 用户菜单定制
  * 角色菜单权限
  * 菜单收藏功能
  * 动态菜单预置

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

* VueVbenAdmin集成
  * 主题设置
  * 布局设置
  * 菜单设置
  * 标题栏设置
  * 多标签页设置

## 项目结构

* `LINGYUN.Platform.Domain.Shared`: 共享领域层
* `LINGYUN.Platform.Domain`: 领域层
* `LINGYUN.Platform.EntityFrameworkCore`: 数据访问层
* `LINGYUN.Platform.Application.Contracts`: 应用服务契约层
* `LINGYUN.Platform.Application`: 应用服务实现层
* `LINGYUN.Platform.HttpApi`: HTTP API层
* `LINGYUN.Platform.Settings.VueVbenAdmin`: VueVbenAdmin前端框架设置模块

## 快速开始

1. 引用模块
```csharp
[DependsOn(
    typeof(PlatformDomainModule),
    typeof(PlatformApplicationModule),
    typeof(PlatformHttpApiModule),
    typeof(PlatformSettingsVueVbenAdminModule)
)]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 配置数据库
```json
{
  "ConnectionStrings": {
    "Platform": "Server=localhost;Database=Platform;Trusted_Connection=True"
  }
}
```

3. 更新数据库
```bash
dotnet ef database update
```

## 注意事项

1. 动态菜单管理
   * 模块默认已初始化与vue-admin相关的菜单
   * 可以通过 `IDataSeedContributor` 接口预置菜单数据
   * 布局(path)和菜单(component)不需要添加 @/ 前缀

2. 数据库迁移
   * 请在运行平台服务之前执行数据库迁移
   * 使用 `dotnet ef database update` 命令更新数据库结构

## 更多信息

* [共享领域层](./LINGYUN.Platform.Domain.Shared/README.md)
* [领域层](./LINGYUN.Platform.Domain/README.md)
* [数据访问层](./LINGYUN.Platform.EntityFrameworkCore/README.md)
* [应用服务契约层](./LINGYUN.Platform.Application.Contracts/README.md)
* [应用服务实现层](./LINGYUN.Platform.Application/README.md)
* [HTTP API层](./LINGYUN.Platform.HttpApi/README.md)
* [VueVbenAdmin设置模块](./LINGYUN.Platform.Settings.VueVbenAdmin/README.md)
