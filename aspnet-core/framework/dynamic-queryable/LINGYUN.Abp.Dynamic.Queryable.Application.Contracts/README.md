# LINGYUN.Abp.Dynamic.Queryable.Application.Contracts

动态查询应用服务契约模块，定义了动态查询相关的接口和DTO。

## 功能特性

* 定义动态查询应用服务接口 `IDynamicQueryableAppService<TEntityDto>`
* 提供动态查询相关的DTO定义
* 支持参数选项和比较运算符的定义

## 配置使用

1. 安装 `LINGYUN.Abp.Dynamic.Queryable.Application.Contracts` NuGet包

2. 添加 `[DependsOn(typeof(AbpDynamicQueryableApplicationContractsModule))]` 到你的模块类

### 接口说明

```csharp
public interface IDynamicQueryableAppService<TEntityDto>
{
    // 获取可用字段列表
    Task<ListResultDto<DynamicParamterDto>> GetAvailableFieldsAsync();

    // 根据动态条件查询数据
    Task<PagedResultDto<TEntityDto>> SearchAsync(GetListByDynamicQueryableInput dynamicInput);
}
```

### DTO说明

* `DynamicParamterDto` - 动态参数DTO
  * `Name` - 字段名称
  * `Type` - 字段类型
  * `Description` - 字段描述
  * `JavaScriptType` - JavaScript类型
  * `AvailableComparator` - 可用的比较运算符
  * `Options` - 参数选项（用于枚举类型）

* `ParamterOptionDto` - 参数选项DTO
  * `Key` - 选项键
  * `Value` - 选项值

* `GetListByDynamicQueryableInput` - 动态查询输入DTO
  * `SkipCount` - 跳过记录数
  * `MaxResultCount` - 最大返回记录数
  * `Queryable` - 查询条件

## 相关链接

* [LINGYUN.Linq.Dynamic.Queryable](../LINGYUN.Linq.Dynamic.Queryable/README.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application](../LINGYUN.Abp.Dynamic.Queryable.Application/README.md)
