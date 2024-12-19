# LINGYUN.Abp.RulesEngineManagement.Application.Contracts

## 1. Introduction

The application service contract layer of the rules engine management module, defining interfaces, DTOs, and other contracts required for rules engine management.

## 2. Features

* Defines application service interfaces for rules engine management
* Defines Data Transfer Objects (DTOs)
* Defines permissions

## 3. Application Service Interfaces

* IRuleRecordAppService
  * Provides CRUD operation interfaces for rule records
  * Supports querying rules by name
  * Supports paginated queries
  
* IWorkflowRecordAppService
  * Provides CRUD operation interfaces for workflow records
  * Supports querying workflows by name and type
  * Supports paginated queries

## 4. Data Transfer Objects

### 4.1 Rule Record DTOs

* RuleRecordDto
* CreateRuleRecordDto
* UpdateRuleRecordDto
* RuleRecordGetListInput

### 4.2 Workflow Record DTOs

* WorkflowRecordDto
* CreateWorkflowRecordDto
* UpdateWorkflowRecordDto
* WorkflowRecordGetListInput

### 4.3 Parameter Record DTOs

* ParamRecordDto
* CreateParamRecordDto
* UpdateParamRecordDto

### 4.4 Action Record DTOs

* ActionRecordDto
* CreateActionRecordDto
* UpdateActionRecordDto

## 5. Permission Definitions

* RulesEngineManagement.Rule
  * Rule management permissions
  * Includes create, modify, delete, query permissions
  
* RulesEngineManagement.Workflow
  * Workflow management permissions
  * Includes create, modify, delete, query permissions

## 6. Dependencies

* Volo.Abp.Ddd.Application.Contracts
* LINGYUN.Abp.RulesEngineManagement.Domain.Shared

[查看中文文档](README.md)
