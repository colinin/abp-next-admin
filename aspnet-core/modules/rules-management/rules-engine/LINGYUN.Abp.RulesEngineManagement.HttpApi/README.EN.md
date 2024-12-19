# LINGYUN.Abp.RulesEngineManagement.HttpApi

## 1. Introduction

The HTTP API layer of the rules engine management module, providing RESTful API interfaces.

## 2. Features

* Implements HTTP API interfaces for rule records
* Implements HTTP API interfaces for workflow records
* Provides API interface routing configuration

## 3. API Interfaces

### 3.1 Rule Record API

* RuleRecordController
  * Base path: api/rules-engine-management/rules
  * Provides CRUD operation APIs for rule records
  * Supports paginated query API
  * Implements permission validation

### 3.2 Workflow Record API

* WorkflowRecordController
  * Base path: api/rules-engine-management/workflows
  * Provides CRUD operation APIs for workflow records
  * Supports paginated query API
  * Implements permission validation

## 4. Dependencies

* Volo.Abp.AspNetCore.Mvc
* LINGYUN.Abp.RulesEngineManagement.Application.Contracts

[查看中文文档](README.md)
