# LINGYUN.Abp.RulesEngineManagement.Domain

## 1. 介绍

规则引擎管理模块的领域层，实现了规则引擎的核心业务逻辑，包括工作流存储、规则记录、动作记录等功能。

## 2. 功能

* 工作流存储
  * 支持基于内存缓存的工作流存储
  * 提供工作流的获取和查询功能
  * 支持按类型获取工作流列表
  
* 规则记录
  * 定义规则记录实体
  * 提供规则记录仓储接口
  * 支持规则的CRUD操作
  
* 工作流规则记录
  * 定义工作流规则记录实体
  * 支持工作流和规则的关联
  
* 参数记录
  * 定义参数记录实体
  * 支持工作流参数的管理
  
* 动作记录
  * 定义动作记录实体
  * 支持成功/失败动作的记录

## 3. 领域服务

* WorkflowStore
  * 实现了IWorkflowStore接口
  * 提供工作流的缓存管理
  * 支持工作流的查询和映射

## 4. 仓储接口

* IRuleRecordRepository
  * 提供规则记录的CRUD操作
  * 支持按名称查询规则
  
* IWorkflowRecordRepository
  * 提供工作流记录的CRUD操作
  * 支持按名称和类型查询工作流

## 5. 依赖

* Volo.Abp.Domain
* LINGYUN.Abp.Rules.RulesEngine
* Microsoft.Extensions.Caching.Memory

[点击查看英文文档](README.EN.md)
