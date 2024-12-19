# LINGYUN.Abp.DataProtectionManagement.Application.Contracts

数据权限管理应用服务契约模块，提供数据权限管理的应用服务接口和DTO。

## 功能

* 数据权限管理应用服务接口
* 数据权限DTO定义
* 数据权限查询定义

## 应用服务接口

* `IDataProtectionAppService` - 数据权限应用服务接口
  * `GetAsync` - 获取数据权限
  * `GetListAsync` - 获取数据权限列表
  * `CreateAsync` - 创建数据权限
  * `UpdateAsync` - 更新数据权限
  * `DeleteAsync` - 删除数据权限

## DTO定义

* `DataProtectionDto` - 数据权限DTO
  * `Id` - 主键
  * `Name` - 名称
  * `DisplayName` - 显示名称
  * `Description` - 描述
  * `AllowProperties` - 允许的属性列表
  * `FilterGroup` - 过滤规则组

* `DataProtectionCreateDto` - 创建数据权限DTO
* `DataProtectionUpdateDto` - 更新数据权限DTO
* `DataProtectionGetListInput` - 获取数据权限列表输入DTO

## 相关链接

* [English document](./README.EN.md)
