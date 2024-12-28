# LINGYUN.Abp.EntityChange.Application

实体变更追踪与恢复的应用层实现模块。

## 功能

* 实现实体变更查询服务
* 实现实体恢复服务
* 提供实体变更的自动映射配置

## 基本用法

```csharp
[DependsOn(typeof(AbpEntityChangeApplicationModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ...
    }
}
```

## 服务实现

* `EntityRestoreAppService<TEntity, TKey>`: 实体恢复服务实现
  * `RestoreEntityAsync`: 通过审计日志恢复单个实体到指定版本
  * `RestoreEntitesAsync`: 通过审计日志批量恢复实体到指定版本
  * 支持通过 `RestorePolicy` 配置恢复权限策略

## 对象映射

模块使用AutoMapper实现以下对象的自动映射：
* `EntityPropertyChange` -> `EntityPropertyChangeDto`
* `EntityChange` -> `EntityChangeDto`

[English](./README.EN.md)
