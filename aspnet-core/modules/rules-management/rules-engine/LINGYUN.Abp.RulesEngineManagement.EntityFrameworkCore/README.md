# LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore

## 1. 介绍

规则引擎管理模块的EntityFrameworkCore实现，提供了基于EF Core的数据访问层实现。

## 2. 功能

* 实现了规则引擎管理模块的DbContext
* 提供了实体的数据库映射配置
* 实现了仓储接口

## 3. 数据库实现

### 3.1 DbContext

* RulesEngineManagementDbContext
  * 实现了IRulesEngineManagementDbContext接口
  * 包含了所有实体的DbSet定义
  * 支持多租户

### 3.2 仓储实现

* EfCoreRuleRecordRepository
  * 实现了IRuleRecordRepository接口
  * 提供规则记录的CRUD操作
  * 支持按名称查询规则
  
* EfCoreWorkflowRecordRepository
  * 实现了IWorkflowRecordRepository接口
  * 提供工作流记录的CRUD操作
  * 支持按名称和类型查询工作流

### 3.3 实体映射

* 工作流记录映射
  * 配置了主键、索引
  * 配置了字段长度限制
  * 配置了关联关系
  
* 规则记录映射
  * 配置了主键、索引
  * 配置了字段长度限制
  * 配置了关联关系
  
* 参数记录映射
  * 配置了主键
  * 配置了字段长度限制
  
* 动作记录映射
  * 配置了主键
  * 配置了字段长度限制

## 4. 依赖

* Volo.Abp.EntityFrameworkCore
* LINGYUN.Abp.RulesEngineManagement.Domain

[点击查看英文文档](README.EN.md)
