# LINGYUN.Abp.EntityChange.Application.Contracts

实体变更追踪与恢复的应用层契约模块。

## 功能

* 提供实体变更查询接口
* 提供实体恢复接口
* 提供实体变更相关DTO对象
* 提供多语言本地化资源

## 基本用法

```csharp
[DependsOn(typeof(AbpEntityChangeApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ...
    }
}
```

## API

* `IEntityChangeAppService`: 实体变更查询服务接口
  * `GetListAsync`: 获取实体变更历史记录列表

* `IEntityRestoreAppService`: 实体恢复服务接口
  * `RestoreEntityAsync`: 恢复单个实体到指定版本
  * `RestoreEntitesAsync`: 批量恢复实体到指定版本

## 本地化

模块提供了以下语言的本地化资源：
* en
* zh-Hans

本地化资源位于 `/LINGYUN/Abp/EntityChange/Localization/Resources` 目录下。

[English](./README.EN.md)
