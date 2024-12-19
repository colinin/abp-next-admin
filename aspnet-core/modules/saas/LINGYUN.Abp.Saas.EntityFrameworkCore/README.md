# LINGYUN.Abp.Saas.EntityFrameworkCore

SaaS EntityFrameworkCore模块，实现了租户和版本的数据访问层。

## 功能特性

* 实体映射配置
  * 租户实体映射
  * 版本实体映射
  * 租户连接字符串实体映射

* 仓储实现
  * EfCoreTenantRepository：租户仓储实现
  * EfCoreEditionRepository：版本仓储实现

* 数据库表
  * AbpEditions：版本表
  * AbpTenants：租户表
  * AbpTenantConnectionStrings：租户连接字符串表

## 配置说明

可以通过配置修改数据库表前缀和Schema：

```json
{
  "AbpSaas": {
    "EntityFrameworkCore": {
      "TablePrefix": "Abp",  // 数据库表前缀
      "Schema": null         // 数据库Schema
    }
  }
}
```

## 更多

[English](README.EN.md)
