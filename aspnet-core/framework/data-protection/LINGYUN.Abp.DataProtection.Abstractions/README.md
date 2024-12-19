# LINGYUN.Abp.DataProtection.Abstractions

数据权限抽象模块，提供数据权限相关的接口定义和基础类型。

## 功能

* `IDataProtected` - 数据权限接口，标记实体需要进行数据权限控制
* `DataProtectedAttribute` - 数据权限特性，标记方法或类需要进行数据权限控制
* `DisableDataProtectedAttribute` - 禁用数据权限特性，标记方法或类不进行数据权限控制

## 数据操作类型

* `DataAccessOperation.Read` - 查询操作
* `DataAccessOperation.Write` - 更新操作
* `DataAccessOperation.Delete` - 删除操作

## 数据过滤

* `DataAccessFilterLogic` - 数据过滤逻辑
  * `And` - 且
  * `Or` - 或
* `DataAccessFilterRule` - 数据过滤规则
  * `Field` - 字段名
  * `Value` - 字段值
  * `Operate` - 操作符
  * `IsLeft` - 是否左括号

## 相关链接

* [English document](./README.EN.md)
