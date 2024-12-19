# LINGYUN.Abp.FeatureManagement.Application

功能管理应用服务模块，提供了功能定义的管理服务实现。

## 功能特性

* 功能定义管理
  * 支持创建、更新、删除功能定义
  * 支持功能定义的本地化
  * 支持功能定义的值类型序列化
* 功能组定义管理
  * 支持创建、更新、删除功能组定义
  * 支持功能组的本地化
* 支持静态和动态功能定义存储
* 集成ABP功能管理模块

## 模块依赖

```csharp
[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(VoloAbpFeatureManagementApplicationModule))]
public class AbpFeatureManagementApplicationModule : AbpModule
{
}
```

## 权限定义

* FeatureManagement.GroupDefinitions
  * FeatureManagement.GroupDefinitions.Create
  * FeatureManagement.GroupDefinitions.Update
  * FeatureManagement.GroupDefinitions.Delete
* FeatureManagement.Definitions
  * FeatureManagement.Definitions.Create
  * FeatureManagement.Definitions.Update
  * FeatureManagement.Definitions.Delete

## 更多信息

* [ABP功能管理文档](https://docs.abp.io/en/abp/latest/Features)
* [功能管理最佳实践](https://docs.abp.io/en/abp/latest/Best-Practices/Features)

[English](README.EN.md)
