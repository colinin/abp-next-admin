# LINGYUN.Abp.FeatureManagement.Application.Contracts

功能管理应用服务契约模块，定义了功能管理所需的接口、DTO和权限。

## 功能特性

* 功能定义管理接口
  * IFeatureDefinitionAppService
  * 支持功能定义的CRUD操作
* 功能组定义管理接口
  * IFeatureGroupDefinitionAppService
  * 支持功能组定义的CRUD操作
* 完整的DTO定义
  * FeatureDefinitionDto
  * FeatureGroupDefinitionDto
  * 创建、更新和查询DTO
* 权限定义
  * 功能定义管理权限
  * 功能组定义管理权限

## 模块依赖

```csharp
[DependsOn(
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(VoloAbpFeatureManagementApplicationContractsModule))]
public class AbpFeatureManagementApplicationContractsModule : AbpModule
{
}
```

## 权限常量

```csharp
public static class FeatureManagementPermissionNames
{
    public const string GroupName = "FeatureManagement";

    public static class GroupDefinition
    {
        public const string Default = GroupName + ".GroupDefinitions";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Definition
    {
        public const string Default = GroupName + ".Definitions";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}
```

## 错误代码

* Error:100001 - 功能定义已存在
* Error:100002 - 功能组定义已存在
* Error:100003 - 无法删除静态功能定义
* Error:100004 - 无法删除静态功能组定义

## 更多信息

* [ABP功能管理文档](https://docs.abp.io/en/abp/latest/Features)
* [ABP应用服务文档](https://docs.abp.io/en/abp/latest/Application-Services)

[English](README.EN.md)
