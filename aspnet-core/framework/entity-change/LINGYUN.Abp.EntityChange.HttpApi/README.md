# LINGYUN.Abp.EntityChange.HttpApi

实体变更追踪与恢复的HTTP API模块。

## 功能

* 提供实体变更查询的HTTP API
* 提供实体恢复的HTTP API
* 支持多语言本地化

## 基本用法

```csharp
[DependsOn(typeof(AbpEntityChangeHttpApiModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ...
    }
}
```

## API端点

* `EntityChangeController`: 实体变更控制器
  * GET `/api/entity-changes`: 获取实体变更历史记录列表

* `EntityRestoreController`: 实体恢复控制器
  * PUT `/api/entity-restore`: 恢复单个实体到指定版本
  * PUT `/api/{EntityId}/entity-restore`: 恢复指定ID的实体
  * PUT `/api/{EntityId}/v/{EntityChangeId}/entity-restore`: 恢复指定ID的实体到指定版本
  * PUT `/api/entites-restore`: 批量恢复实体到指定版本

## 本地化

模块使用 `AbpEntityChangeResource` 作为本地化资源，支持多语言显示。

[English](./README.EN.md)
