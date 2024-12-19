# LINGYUN.Abp.Auditing.Application.Contracts

Application layer contracts module for audit logging, defining application service interfaces and data transfer objects.

[简体中文](./README.md)

## Features

* Audit log application service interface definitions
* Audit log Data Transfer Objects (DTOs) definitions
* Audit log permission definitions
* Audit log query parameter definitions

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuditingApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Permission Definitions

* AuditLogging.AuditLogs - Audit log management
  - AuditLogging.AuditLogs.Delete - Delete audit logs
  - AuditLogging.SecurityLogs - Security log management
  - AuditLogging.SecurityLogs.Delete - Delete security logs

## Service Interfaces

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

## Data Transfer Objects

* AuditLogDto - Audit log DTO
* SecurityLogDto - Security log DTO
* GetAuditLogsInput - Input parameters for getting audit logs
* GetSecurityLogsInput - Input parameters for getting security logs
* DeleteManyAuditLogsInput - Input parameters for batch deleting audit logs
* DeleteManySecurityLogsInput - Input parameters for batch deleting security logs
