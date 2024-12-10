# LINGYUN.Abp.DataProtectionManagement.HttpApi

数据权限管理HTTP API模块，提供数据权限管理的REST API接口。

## 功能

* 数据权限管理REST API
  * 创建数据权限
  * 更新数据权限
  * 删除数据权限
  * 查询数据权限

## API控制器

* `DataProtectionController` - 数据权限控制器
  * `GET /api/data-protection-management/data-protection/{id}` - 获取指定数据权限
  * `GET /api/data-protection-management/data-protection` - 获取数据权限列表
  * `POST /api/data-protection-management/data-protection` - 创建数据权限
  * `PUT /api/data-protection-management/data-protection/{id}` - 更新数据权限
  * `DELETE /api/data-protection-management/data-protection/{id}` - 删除数据权限

## 权限验证

* 所有API都需要认证
* 创建数据权限需要 `DataProtectionManagement.DataProtection.Create` 权限
* 更新数据权限需要 `DataProtectionManagement.DataProtection.Update` 权限
* 删除数据权限需要 `DataProtectionManagement.DataProtection.Delete` 权限
* 查询数据权限需要 `DataProtectionManagement.DataProtection` 权限

## 相关链接

* [English document](./README.EN.md)
