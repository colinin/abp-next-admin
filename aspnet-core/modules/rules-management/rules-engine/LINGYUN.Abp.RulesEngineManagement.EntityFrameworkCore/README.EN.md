# LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore

## 1. Introduction

The EntityFrameworkCore implementation of the rules engine management module, providing EF Core-based data access layer implementation.

## 2. Features

* Implements DbContext for the rules engine management module
* Provides entity database mapping configuration
* Implements repository interfaces

## 3. Database Implementation

### 3.1 DbContext

* RulesEngineManagementDbContext
  * Implements IRulesEngineManagementDbContext interface
  * Contains DbSet definitions for all entities
  * Supports multi-tenancy

### 3.2 Repository Implementation

* EfCoreRuleRecordRepository
  * Implements IRuleRecordRepository interface
  * Provides CRUD operations for rule records
  * Supports querying rules by name
  
* EfCoreWorkflowRecordRepository
  * Implements IWorkflowRecordRepository interface
  * Provides CRUD operations for workflow records
  * Supports querying workflows by name and type

### 3.3 Entity Mapping

* Workflow Record Mapping
  * Configures primary keys and indexes
  * Configures field length restrictions
  * Configures relationships
  
* Rule Record Mapping
  * Configures primary keys and indexes
  * Configures field length restrictions
  * Configures relationships
  
* Parameter Record Mapping
  * Configures primary keys
  * Configures field length restrictions
  
* Action Record Mapping
  * Configures primary keys
  * Configures field length restrictions

## 4. Dependencies

* Volo.Abp.EntityFrameworkCore
* LINGYUN.Abp.RulesEngineManagement.Domain

[查看中文文档](README.md)
