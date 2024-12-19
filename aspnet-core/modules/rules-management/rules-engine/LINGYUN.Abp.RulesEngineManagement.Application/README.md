# LINGYUN.Abp.RulesEngineManagement.Application

## 1. 介绍

规则引擎管理模块的应用服务实现层，实现了规则引擎管理的业务逻辑。

## 2. 功能

* 实现了规则记录的应用服务
* 实现了工作流记录的应用服务
* 提供了规则引擎管理的自动映射配置

## 3. 应用服务实现

### 3.1 规则记录应用服务

* RuleRecordAppService
  * 实现了IRuleRecordAppService接口
  * 提供规则记录的CRUD操作
  * 实现了规则记录的查询功能
  * 支持分页查询
  * 实现了权限验证

### 3.2 工作流记录应用服务

* WorkflowRecordAppService
  * 实现了IWorkflowRecordAppService接口
  * 提供工作流记录的CRUD操作
  * 实现了工作流记录的查询功能
  * 支持分页查询
  * 实现了权限验证

## 4. 对象映射

* RulesEngineManagementApplicationAutoMapperProfile
  * 配置了DTO与实体间的自动映射
  * 包含了所有规则引擎管理相关的映射配置

## 5. 依赖

* Volo.Abp.AutoMapper
* Volo.Abp.Ddd.Application
* LINGYUN.Abp.RulesEngineManagement.Application.Contracts
* LINGYUN.Abp.RulesEngineManagement.Domain

[点击查看英文文档](README.EN.md)
