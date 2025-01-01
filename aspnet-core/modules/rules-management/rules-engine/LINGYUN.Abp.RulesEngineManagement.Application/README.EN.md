# LINGYUN.Abp.RulesEngineManagement.Application

## 1. Introduction

The application service implementation layer of the rules engine management module, implementing the business logic for rules engine management.

## 2. Features

* Implements rule record application services
* Implements workflow record application services
* Provides auto-mapping configuration for rules engine management

## 3. Application Service Implementation

### 3.1 Rule Record Application Service

* RuleRecordAppService
  * Implements IRuleRecordAppService interface
  * Provides CRUD operations for rule records
  * Implements rule record query functionality
  * Supports paginated queries
  * Implements permission validation

### 3.2 Workflow Record Application Service

* WorkflowRecordAppService
  * Implements IWorkflowRecordAppService interface
  * Provides CRUD operations for workflow records
  * Implements workflow record query functionality
  * Supports paginated queries
  * Implements permission validation

## 4. Object Mapping

* RulesEngineManagementApplicationAutoMapperProfile
  * Configures automatic mapping between DTOs and entities
  * Includes all mapping configurations related to rules engine management

## 5. Dependencies

* Volo.Abp.AutoMapper
* Volo.Abp.Ddd.Application
* LINGYUN.Abp.RulesEngineManagement.Application.Contracts
* LINGYUN.Abp.RulesEngineManagement.Domain

[查看中文文档](README.md)
