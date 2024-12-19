# LINGYUN.Abp.RulesEngineManagement.Domain.Shared

## 1. 介绍

规则引擎管理模块的共享领域层，包含了规则引擎管理所需的常量定义、枚举类型等基础设施。

## 2. 功能

* 定义了工作流记录相关的常量
* 定义了规则记录相关的常量
* 定义了动作记录相关的常量
* 定义了参数记录相关的常量
* 定义了动作类型枚举

## 3. 常量配置

### 3.1 工作流记录常量

* MaxNameLength: 工作流名称最大长度，默认64
* MaxTypeFullNameLength: 类型全名最大长度，默认255
* MaxInjectWorkflowsLength: 注入工作流最大长度，默认为(MaxNameLength + 1) * 5

### 3.2 规则记录常量

* MaxNameLength: 规则名称最大长度，默认64
* MaxOperatorLength: 操作符最大长度，默认30
* MaxErrorMessageLength: 错误消息最大长度，默认255
* MaxInjectWorkflowsLength: 注入工作流最大长度，默认为(MaxNameLength + 1) * 5
* MaxExpressionLength: 表达式最大长度，默认为int.MaxValue
* MaxSuccessEventLength: 成功事件最大长度，默认128

### 3.3 动作类型枚举

* Success = 0: 成功
* Failure = 1: 失败

## 4. 依赖

* Volo.Abp.Validation
* LINGYUN.Abp.Rules.RulesEngine

[点击查看英文文档](README.EN.md)
