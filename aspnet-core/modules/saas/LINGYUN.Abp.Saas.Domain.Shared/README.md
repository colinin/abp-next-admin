# LINGYUN.Abp.Saas.Domain.Shared

SaaS领域共享层模块，定义了租户和版本的常量、枚举、事件等共享内容。

## 功能特性

* 常量定义
  * EditionConsts：版本相关常量
  * TenantConsts：租户相关常量
  * TenantConnectionStringConsts：租户连接字符串相关常量

* 枚举定义
  * RecycleStrategy：资源回收策略
    * Reserve：保留
    * Recycle：回收

* 事件定义
  * EditionEto：版本事件传输对象
  * TenantEto：租户事件传输对象

* 本地化资源
  * 多语言支持(en/zh-Hans)
  * 错误消息
  * 权限描述
  * 功能描述

## 更多

[English](README.EN.md)
