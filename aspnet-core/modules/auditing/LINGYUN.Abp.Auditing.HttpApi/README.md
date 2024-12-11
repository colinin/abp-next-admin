# LINGYUN.Abp.Auditing.HttpApi

审计日志HTTP API模块，提供审计日志的REST API接口。

[English](./README.EN.md)

## 功能特性

* 审计日志REST API接口
* 安全日志REST API接口
* 支持API版本控制
* 集成Swagger文档

## 模块引用

```csharp
[DependsOn(typeof(AbpAuditingHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API接口

### 审计日志

* GET /api/audit-logging/audit-logs/{id} - 获取指定的审计日志
* GET /api/audit-logging/audit-logs - 获取审计日志列表
* DELETE /api/audit-logging/audit-logs/{id} - 删除指定的审计日志
* DELETE /api/audit-logging/audit-logs/batch - 批量删除审计日志

### 安全日志

* GET /api/audit-logging/security-logs/{id} - 获取指定的安全日志
* GET /api/audit-logging/security-logs - 获取安全日志列表
* DELETE /api/audit-logging/security-logs/{id} - 删除指定的安全日志
* DELETE /api/audit-logging/security-logs/batch - 批量删除安全日志

## 基本用法

1. 引用模块
2. 配置权限（如需要）
3. 通过HTTP客户端调用相应的API接口

## API文档

启动应用程序后，可以通过Swagger UI查看完整的API文档：
* /swagger - Swagger UI界面
* /swagger/v1/swagger.json - OpenAPI规范文档
