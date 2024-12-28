# LINGYUN.Abp.DataProtectionManagement.Application

数据权限管理应用服务模块，提供数据权限管理的应用服务实现。

## 功能

* 数据权限管理应用服务
  * 创建数据权限
  * 更新数据权限
  * 删除数据权限
  * 查询数据权限
* 自动映射配置
* 权限验证

## 应用服务实现

* `DataProtectionAppService` - 数据权限应用服务
  * 实现 `IDataProtectionAppService` 接口
  * 提供数据权限的CRUD操作
  * 包含权限验证
  * 包含数据验证

## 自动映射配置

* `DataProtectionManagementApplicationAutoMapperProfile` - 自动映射配置文件
  * `DataProtection` -> `DataProtectionDto`
  * `DataProtectionCreateDto` -> `DataProtection`
  * `DataProtectionUpdateDto` -> `DataProtection`

## 权限验证

* 创建数据权限需要 `DataProtectionManagement.DataProtection.Create` 权限
* 更新数据权限需要 `DataProtectionManagement.DataProtection.Update` 权限
* 删除数据权限需要 `DataProtectionManagement.DataProtection.Delete` 权限
* 查询数据权限需要 `DataProtectionManagement.DataProtection` 权限

## 相关链接

* [English document](./README.EN.md)
