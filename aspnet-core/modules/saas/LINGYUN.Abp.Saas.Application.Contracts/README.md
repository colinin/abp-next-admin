# LINGYUN.Abp.Saas.Application.Contracts

SaaS应用服务契约模块，定义了租户和版本管理的应用服务接口、DTO对象和权限定义。

## 功能特性

* 租户管理接口(ITenantAppService)
  * 定义了租户管理的所有服务接口
  * 包含租户相关的DTO对象定义
  * 租户连接字符串管理接口

* 版本管理接口(IEditionAppService)
  * 定义了版本管理的所有服务接口
  * 包含版本相关的DTO对象定义

* 权限定义(AbpSaasPermissions)
  * 版本管理权限
  * 租户管理权限
  * 功能管理权限
  * 连接字符串管理权限

* DTO对象
  * EditionCreateDto/EditionUpdateDto
  * TenantCreateDto/TenantUpdateDto
  * TenantConnectionStringCreateDto/TenantConnectionStringUpdateDto

## 更多

[English](README.EN.md)
