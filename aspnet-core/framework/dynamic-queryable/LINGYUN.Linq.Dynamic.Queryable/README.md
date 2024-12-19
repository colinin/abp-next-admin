# LINGYUN.Linq.Dynamic.Queryable

动态查询基本库, 扩展Linq, 动态构建表达式树  

## 功能特性

* 支持动态构建查询条件
* 支持多种比较运算符：等于、不等于、大于、小于、大于等于、小于等于、包含、不包含、开始于、不开始于、结束于、不结束于等
* 支持动态类型转换和空值处理
* 支持枚举类型的动态查询

## 配置使用

模块按需引用, 只提供针对Linq的扩展。

### 基本用法

```csharp
// 创建动态查询参数
var parameter = new DynamicParamter 
{
    Field = "Name",
    Comparison = DynamicComparison.Equal,
    Value = "test"
};

// 创建查询条件
var queryable = new DynamicQueryable
{
    Paramters = new List<DynamicParamter> { parameter }
};

// 应用到Expression
Expression<Func<TEntity, bool>> condition = e => true;
condition = condition.DynamicQuery(queryable);

// 使用条件进行查询
var result = await repository.Where(condition).ToListAsync();
```

### 支持的比较运算符

* `Equal` - 等于
* `NotEqual` - 不等于
* `LessThan` - 小于
* `LessThanOrEqual` - 小于等于
* `GreaterThan` - 大于
* `GreaterThanOrEqual` - 大于等于
* `Contains` - 包含
* `NotContains` - 不包含
* `StartsWith` - 开始于
* `NotStartsWith` - 不开始于
* `EndsWith` - 结束于
* `NotEndsWith` - 不结束于

## 相关链接

* [LINGYUN.Abp.Dynamic.Queryable.Application](../LINGYUN.Abp.Dynamic.Queryable.Application/README.md)
