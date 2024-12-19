# LINGYUN.Abp.RulesEngineManagement.Domain.Shared

## 1. Introduction

The shared domain layer of the rules engine management module, containing constant definitions, enumeration types, and other infrastructure required for rules engine management.

## 2. Features

* Defines workflow record related constants
* Defines rule record related constants
* Defines action record related constants
* Defines parameter record related constants
* Defines action type enumeration

## 3. Constants Configuration

### 3.1 Workflow Record Constants

* MaxNameLength: Maximum length of workflow name, default 64
* MaxTypeFullNameLength: Maximum length of type full name, default 255
* MaxInjectWorkflowsLength: Maximum length of inject workflows, default (MaxNameLength + 1) * 5

### 3.2 Rule Record Constants

* MaxNameLength: Maximum length of rule name, default 64
* MaxOperatorLength: Maximum length of operator, default 30
* MaxErrorMessageLength: Maximum length of error message, default 255
* MaxInjectWorkflowsLength: Maximum length of inject workflows, default (MaxNameLength + 1) * 5
* MaxExpressionLength: Maximum length of expression, default int.MaxValue
* MaxSuccessEventLength: Maximum length of success event, default 128

### 3.3 Action Type Enumeration

* Success = 0: Success
* Failure = 1: Failure

## 4. Dependencies

* Volo.Abp.Validation
* LINGYUN.Abp.Rules.RulesEngine

[查看中文文档](README.md)
