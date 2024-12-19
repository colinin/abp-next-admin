# LINGYUN.Abp.RulesEngineManagement.HttpApi

## 1. 介绍

规则引擎管理模块的HTTP API层，提供了基于RESTful的API接口。

## 2. 功能

* 实现了规则记录的HTTP API接口
* 实现了工作流记录的HTTP API接口
* 提供了API接口的路由配置

## 3. API接口

### 3.1 规则记录API

* RuleRecordController
  * 基路径: api/rules-engine-management/rules
  * 提供规则记录的CRUD操作API
  * 支持分页查询API
  * 实现了权限验证

### 3.2 工作流记录API

* WorkflowRecordController
  * 基路径: api/rules-engine-management/workflows
  * 提供工作流记录的CRUD操作API
  * 支持分页查询API
  * 实现了权限验证

## 4. 依赖

* Volo.Abp.AspNetCore.Mvc
* LINGYUN.Abp.RulesEngineManagement.Application.Contracts

[点击查看英文文档](README.EN.md)
