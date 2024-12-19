# LINGYUN.Abp.FeatureManagement.HttpApi

功能管理HTTP API模块，提供了功能定义管理的REST API接口。

## 功能特性

* 功能定义管理API
  * 创建、更新、删除功能定义
  * 查询功能定义列表
* 功能组定义管理API
  * 创建、更新、删除功能组定义
  * 查询功能组定义列表
* 支持本地化
* 集成ABP功能管理模块

## 模块依赖

```csharp
[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(VoloAbpFeatureManagementHttpApiModule))]
public class AbpFeatureManagementHttpApiModule : AbpModule
{
}
```

## API路由

### 功能定义

* GET /api/feature-management/definitions
* GET /api/feature-management/definitions/{name}
* POST /api/feature-management/definitions
* PUT /api/feature-management/definitions/{name}
* DELETE /api/feature-management/definitions/{name}

### 功能组定义

* GET /api/feature-management/group-definitions
* GET /api/feature-management/group-definitions/{name}
* POST /api/feature-management/group-definitions
* PUT /api/feature-management/group-definitions/{name}
* DELETE /api/feature-management/group-definitions/{name}

## 本地化配置

模块使用ABP的本地化系统，主要使用以下资源：
* AbpFeatureManagementResource
* AbpValidationResource

## 更多信息

* [ABP Web API文档](https://docs.abp.io/en/abp/latest/API/Auto-API-Controllers)
* [ABP功能管理文档](https://docs.abp.io/en/abp/latest/Features)

[English](README.EN.md)
