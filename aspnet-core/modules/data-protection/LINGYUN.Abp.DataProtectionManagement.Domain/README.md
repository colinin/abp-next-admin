# LINGYUN.Abp.DataProtectionManagement.Domain

数据权限管理领域模块，提供数据权限管理的核心业务逻辑。

## 功能

* 数据权限管理
  * 创建数据权限
  * 更新数据权限
  * 删除数据权限
  * 查询数据权限
* 数据权限资源管理
  * 资源定义
  * 资源分组
  * 资源属性
* 数据权限规则管理
  * 规则定义
  * 规则分组
  * 规则操作符

## 领域服务

* `IDataProtectionManager` - 数据权限管理服务
  * `CreateAsync` - 创建数据权限
  * `UpdateAsync` - 更新数据权限
  * `DeleteAsync` - 删除数据权限
  * `GetAsync` - 获取数据权限
  * `GetListAsync` - 获取数据权限列表

## 实体

* `DataProtection` - 数据权限实体
  * `Id` - 主键
  * `Name` - 名称
  * `DisplayName` - 显示名称
  * `Description` - 描述
  * `AllowProperties` - 允许的属性列表
  * `FilterGroup` - 过滤规则组

## 仓储

* `IDataProtectionRepository` - 数据权限仓储接口
  * `GetListAsync` - 获取数据权限列表
  * `FindByNameAsync` - 根据名称查找数据权限
  * `GetCountAsync` - 获取数据权限数量

## 相关链接

* [English document](./README.EN.md)
