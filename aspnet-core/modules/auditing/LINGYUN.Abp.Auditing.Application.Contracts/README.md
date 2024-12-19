# LINGYUN.Abp.Auditing.Application.Contracts

审计日志应用层契约模块，定义审计日志的应用服务接口和数据传输对象。

[English](./README.EN.md)

## 功能特性

* 审计日志应用服务接口定义
* 审计日志数据传输对象（DTO）定义
* 审计日志权限定义
* 审计日志查询参数定义

## 模块引用

```csharp
[DependsOn(typeof(AbpAuditingApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 权限定义

* AuditLogging.AuditLogs - 审计日志管理
  - AuditLogging.AuditLogs.Delete - 删除审计日志
  - AuditLogging.SecurityLogs - 安全日志管理
  - AuditLogging.SecurityLogs.Delete - 删除安全日志

## 服务接口

* IAuditLogAppService
  ```csharp
  public interface IAuditLogAppService : IApplicationService
  {
      Task<AuditLogDto> GetAsync(Guid id);
      Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogsInput input);
      Task DeleteAsync(Guid id);
      Task DeleteManyAsync(DeleteManyAuditLogsInput input);
  }
  ```

* ISecurityLogAppService
  ```csharp
  public interface ISecurityLogAppService : IApplicationService
  {
      Task<SecurityLogDto> GetAsync(Guid id);
      Task<PagedResultDto<SecurityLogDto>> GetListAsync(GetSecurityLogsInput input);
      Task DeleteAsync(Guid id);
      Task DeleteManyAsync(DeleteManySecurityLogsInput input);
  }
  ```

## 数据传输对象

* AuditLogDto - 审计日志DTO
* SecurityLogDto - 安全日志DTO
* GetAuditLogsInput - 获取审计日志输入参数
* GetSecurityLogsInput - 获取安全日志输入参数
* DeleteManyAuditLogsInput - 批量删除审计日志输入参数
* DeleteManySecurityLogsInput - 批量删除安全日志输入参数
