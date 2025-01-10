# LINGYUN.Abp.Saas.Domain

SaaS领域层模块，定义了租户和版本的核心领域模型、仓储接口和领域服务。

## 核心功能

* 租户实体(Tenant)：包含租户基本信息、状态、过期时间等
* 版本实体(Edition)：包含版本基本信息
* 租户连接字符串(TenantConnectionString)：管理租户数据库连接
* 租户管理器(TenantManager)：处理租户相关的业务逻辑
* 版本管理器(EditionManager)：处理版本相关的业务逻辑
* 数据种子(EditionDataSeeder)：提供默认版本数据初始化

## 领域事件

* 租户创建、更新、删除事件
* 版本创建、更新、删除事件
* 租户连接字符串变更事件

## 缓存管理

* 租户缓存：缓存租户信息，提高查询性能
* 版本缓存：缓存版本信息，提高查询性能

## 更多

[English](README.EN.md)
