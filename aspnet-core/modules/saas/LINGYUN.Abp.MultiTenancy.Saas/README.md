# LINGYUN.Abp.MultiTenancy.Saas

多租户SaaS管理模块，提供租户管理、版本管理等功能。

## 功能特性

* 租户管理：创建、编辑、删除租户，管理租户连接字符串
* 版本管理：创建、编辑、删除版本，管理版本功能
* 租户过期管理：支持租户过期时间设置、过期预警、过期资源回收
* 租户功能管理：支持为租户分配功能权限

## 配置使用

### 模块配置

```json
{
  "AbpSaas": {
    "Tenants": {
      "RecycleStrategy": "1",           // 资源回收策略：0-保留，1-回收
      "ExpirationReminderDays": "15",   // 过期预警天数，范围1-30天
      "ExpiredRecoveryTime": "15"       // 过期回收时长，范围1-30天
    }
  }
}
```

### 权限配置

* AbpSaas.Editions
  * AbpSaas.Editions.Create：创建版本
  * AbpSaas.Editions.Update：更新版本
  * AbpSaas.Editions.Delete：删除版本
  * AbpSaas.Editions.ManageFeatures：管理版本功能

* AbpSaas.Tenants
  * AbpSaas.Tenants.Create：创建租户
  * AbpSaas.Tenants.Update：更新租户
  * AbpSaas.Tenants.Delete：删除租户
  * AbpSaas.Tenants.ManageFeatures：管理租户功能
  * AbpSaas.Tenants.ManageConnectionStrings：管理租户连接字符串

## 更多

[English](README.EN.md)
