# LINGYUN.Abp.Auditing.Application

审计日志应用层模块，提供审计日志的应用服务实现。

[English](./README.EN.md)

## 功能特性

* 审计日志查询服务
* 审计日志管理服务
* 安全日志查询服务
* 集成自动映射功能

## 模块引用

```csharp
[DependsOn(typeof(AbpAuditingApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 依赖模块

* `LINGYUN.Abp.AuditLogging` - 审计日志核心模块
* `LINGYUN.Abp.Logging` - 日志基础模块
* `AbpAutoMapper` - ABP自动映射模块

## 服务接口

* IAuditLogAppService - 审计日志应用服务
  - GetAsync - 获取指定的审计日志
  - GetListAsync - 获取审计日志列表
  - DeleteAsync - 删除指定的审计日志
  - DeleteManyAsync - 批量删除审计日志

* ISecurityLogAppService - 安全日志应用服务
  - GetAsync - 获取指定的安全日志
  - GetListAsync - 获取安全日志列表
  - DeleteAsync - 删除指定的安全日志
  - DeleteManyAsync - 批量删除安全日志

## 基本用法

1. 引用模块
2. 注入所需的应用服务
3. 调用相应的服务方法

示例：
```csharp
public class YourService
{
    private readonly IAuditLogAppService _auditLogAppService;

    public YourService(IAuditLogAppService auditLogAppService)
    {
        _auditLogAppService = auditLogAppService;
    }

    public async Task<AuditLogDto> GetAuditLogAsync(Guid id)
    {
        return await _auditLogAppService.GetAsync(id);
    }
}
```
