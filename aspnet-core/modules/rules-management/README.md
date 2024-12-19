# LINGYUN.Abp.RulesManagement

规则引擎管理模块，提供了基于规则引擎的工作流和规则管理功能。

## 1. 模块说明

该模块基于RulesEngine实现，提供了工作流规则的持久化管理、缓存管理以及API接口。主要包含以下子模块：

### 1.1 Domain.Shared

* 定义了工作流记录、规则记录、动作记录等基础常量
* 提供了动作类型等枚举定义
* [查看详细说明](rules-engine/LINGYUN.Abp.RulesEngineManagement.Domain.Shared/README.md)

### 1.2 Domain

* 实现了工作流存储的核心业务逻辑
* 提供了基于内存缓存的工作流存储实现
* 定义了规则记录、工作流记录等领域模型
* [查看详细说明](rules-engine/LINGYUN.Abp.RulesEngineManagement.Domain/README.md)

### 1.3 EntityFrameworkCore

* 提供了基于EF Core的数据访问层实现
* 实现了工作流、规则等实体的数据库映射
* 实现了仓储接口
* [查看详细说明](rules-engine/LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore/README.md)

### 1.4 Application.Contracts

* 定义了规则引擎管理的应用服务接口
* 定义了数据传输对象（DTOs）
* 定义了权限
* [查看详细说明](rules-engine/LINGYUN.Abp.RulesEngineManagement.Application.Contracts/README.md)

### 1.5 Application

* 实现了规则记录和工作流记录的应用服务
* 提供了对象自动映射配置
* [查看详细说明](rules-engine/LINGYUN.Abp.RulesEngineManagement.Application/README.md)

### 1.6 HttpApi

* 提供了RESTful风格的API接口
* 实现了规则和工作流的CRUD操作API
* [查看详细说明](rules-engine/LINGYUN.Abp.RulesEngineManagement.HttpApi/README.md)

## 2. 功能特性

* 工作流管理
  * 支持工作流的创建、修改、删除、查询
  * 提供工作流的缓存管理
  * 支持按类型查询工作流
  
* 规则管理
  * 支持规则的创建、修改、删除、查询
  * 支持规则表达式的定义
  * 支持规则与工作流的关联
  
* 参数管理
  * 支持工作流参数的定义
  * 支持参数值的验证
  
* 动作管理
  * 支持成功/失败动作的定义
  * 支持动作执行结果的记录

## 3. 配置项

### 3.1 工作流记录配置

* MaxNameLength: 工作流名称最大长度，默认64
* MaxTypeFullNameLength: 类型全名最大长度，默认255
* MaxInjectWorkflowsLength: 注入工作流最大长度，默认(MaxNameLength + 1) * 5

### 3.2 规则记录配置

* MaxNameLength: 规则名称最大长度，默认64
* MaxOperatorLength: 操作符最大长度，默认30
* MaxErrorMessageLength: 错误消息最大长度，默认255
* MaxExpressionLength: 表达式最大长度，默认int.MaxValue
* MaxSuccessEventLength: 成功事件最大长度，默认128

## 4. 权限

* RulesEngineManagement.Rule
  * 规则管理权限
  * 包含创建、修改、删除、查询权限
  
* RulesEngineManagement.Workflow
  * 工作流管理权限
  * 包含创建、修改、删除、查询权限

## 5. API接口

### 5.1 规则记录API

* 基路径: api/rules-engine-management/rules
* 提供规则记录的CRUD操作API
* 支持分页查询API

### 5.2 工作流记录API

* 基路径: api/rules-engine-management/workflows
* 提供工作流记录的CRUD操作API
* 支持分页查询API

## 6. 依赖

* Volo.Abp.Core
* LINGYUN.Abp.Rules.RulesEngine
* Microsoft.Extensions.Caching.Memory
* Volo.Abp.EntityFrameworkCore
* Volo.Abp.Ddd.Application
* Volo.Abp.AspNetCore.Mvc