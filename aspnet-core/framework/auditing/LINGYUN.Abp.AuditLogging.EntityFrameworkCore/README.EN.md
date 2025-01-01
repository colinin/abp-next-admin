# LINGYUN.Abp.AuditLogging.EntityFrameworkCore

[简体中文](./README.md) | English

EntityFrameworkCore implementation for the audit logging module. This module serves as a bridge, with the actual implementation delegated to the official ABP modules.

## Features

* AuditLogManager - Implements IAuditLogManager, audit logs are managed by the Volo.Abp.AuditLogging module
* SecurityLogManager - Implements ISecurityLogManager, security logs are managed by the Volo.Abp.Identity module

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuditLoggingEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

Please follow the configuration guidelines in the Volo.Abp.AuditLogging and Volo.Abp.Identity modules.

## Database Configuration

Configure the connection strings in your appsettings.json:

```json
{
  "ConnectionStrings": {
    "AbpIdentity": "Server=127.0.0.1;Database=Identity;User Id=root;Password=*",
    "AbpAuditLogging": "Server=127.0.0.1;Database=AuditLogging;User Id=root;Password=*",
  }
}
```

## Features

1. Audit Log Management
   - Store and retrieve audit logs using EntityFrameworkCore
   - Support for entity change tracking
   - Integration with ABP's identity system for security logs

2. Auto Mapping
   - Automatic mapping configuration for audit log entities
   - Supports custom mapping profiles through AbpAutoMapperOptions
