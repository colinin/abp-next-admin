# LINGYUN.Abp.Auditing.Application

Application layer module for audit logging, providing implementation of audit logging application services.

[简体中文](./README.md)

## Features

* Audit log query service
* Audit log management service
* Security log query service
* Integration with auto-mapping functionality

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuditingApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Required Modules

* `LINGYUN.Abp.AuditLogging` - Audit logging core module
* `LINGYUN.Abp.Logging` - Logging infrastructure module
* `AbpAutoMapper` - ABP auto-mapping module

## Service Interfaces

* IAuditLogAppService - Audit log application service
  - GetAsync - Get a specific audit log
  - GetListAsync - Get a list of audit logs
  - DeleteAsync - Delete a specific audit log
  - DeleteManyAsync - Batch delete audit logs

* ISecurityLogAppService - Security log application service
  - GetAsync - Get a specific security log
  - GetListAsync - Get a list of security logs
  - DeleteAsync - Delete a specific security log
  - DeleteManyAsync - Batch delete security logs

## Basic Usage

1. Reference the module
2. Inject the required application service
3. Call the appropriate service methods

Example:
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
