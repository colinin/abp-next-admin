# LINGYUN.Abp.Saas.Application

SaaS应用服务层模块，实现了租户和版本管理的应用服务接口。

## 功能特性

* 租户管理服务(TenantAppService)
  * 创建租户
  * 更新租户
  * 删除租户
  * 获取租户列表
  * 获取租户详情
  * 管理租户连接字符串
  * 管理租户功能

* 版本管理服务(EditionAppService)
  * 创建版本
  * 更新版本
  * 删除版本
  * 获取版本列表
  * 获取版本详情
  * 管理版本功能

## 权限验证

所有应用服务方法都已添加相应的权限验证，确保只有具有对应权限的用户才能访问。

## 对象映射

使用AutoMapper实现了以下对象映射：
* Edition <-> EditionDto
* Tenant <-> TenantDto
* TenantConnectionString <-> TenantConnectionStringDto

## 更多

[English](README.EN.md)
