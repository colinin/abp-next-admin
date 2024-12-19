# LINGYUN.Abp.RulesEngineManagement.Application.Contracts

## 1. 介绍

规则引擎管理模块的应用服务契约层，定义了规则引擎管理所需的接口、DTO等。

## 2. 功能

* 定义了规则引擎管理的应用服务接口
* 定义了数据传输对象（DTOs）
* 定义了权限

## 3. 应用服务接口

* IRuleRecordAppService
  * 提供规则记录的CRUD操作接口
  * 支持按名称查询规则
  * 支持分页查询
  
* IWorkflowRecordAppService
  * 提供工作流记录的CRUD操作接口
  * 支持按名称和类型查询工作流
  * 支持分页查询

## 4. 数据传输对象

### 4.1 规则记录DTOs

* RuleRecordDto
* CreateRuleRecordDto
* UpdateRuleRecordDto
* RuleRecordGetListInput

### 4.2 工作流记录DTOs

* WorkflowRecordDto
* CreateWorkflowRecordDto
* UpdateWorkflowRecordDto
* WorkflowRecordGetListInput

### 4.3 参数记录DTOs

* ParamRecordDto
* CreateParamRecordDto
* UpdateParamRecordDto

### 4.4 动作记录DTOs

* ActionRecordDto
* CreateActionRecordDto
* UpdateActionRecordDto

## 5. 权限定义

* RulesEngineManagement.Rule
  * 规则管理权限
  * 包含创建、修改、删除、查询权限
  
* RulesEngineManagement.Workflow
  * 工作流管理权限
  * 包含创建、修改、删除、查询权限

## 6. 依赖

* Volo.Abp.Ddd.Application.Contracts
* LINGYUN.Abp.RulesEngineManagement.Domain.Shared

[点击查看英文文档](README.EN.md)
