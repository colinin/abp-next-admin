# LINGYUN.Abp.RulesEngineManagement.Domain

## 1. Introduction

The domain layer of the rules engine management module implements the core business logic of the rules engine, including workflow storage, rule records, action records, and other functionalities.

## 2. Features

* Workflow Storage
  * Supports memory-based workflow caching
  * Provides workflow retrieval and query functionality
  * Supports getting workflow lists by type
  
* Rule Records
  * Defines rule record entities
  * Provides rule record repository interface
  * Supports CRUD operations for rules
  
* Workflow Rule Records
  * Defines workflow rule record entities
  * Supports association between workflows and rules
  
* Parameter Records
  * Defines parameter record entities
  * Supports workflow parameter management
  
* Action Records
  * Defines action record entities
  * Supports recording success/failure actions

## 3. Domain Services

* WorkflowStore
  * Implements IWorkflowStore interface
  * Provides workflow cache management
  * Supports workflow querying and mapping

## 4. Repository Interfaces

* IRuleRecordRepository
  * Provides CRUD operations for rule records
  * Supports querying rules by name
  
* IWorkflowRecordRepository
  * Provides CRUD operations for workflow records
  * Supports querying workflows by name and type

## 5. Dependencies

* Volo.Abp.Domain
* LINGYUN.Abp.Rules.RulesEngine
* Microsoft.Extensions.Caching.Memory

[查看中文文档](README.md)
