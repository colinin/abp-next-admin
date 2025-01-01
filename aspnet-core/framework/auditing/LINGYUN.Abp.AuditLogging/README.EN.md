# LINGYUN.Abp.AuditLogging

Audit logging core module, providing basic functionality and interface definitions for audit logging.

[简体中文](./README.md)

## Features

* Audit logging infrastructure
* Audit log repository interface definitions
* Audit log manager interface definitions
* Support for ignoring specific types in audit logging

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuditLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Auditing": {
    "IsEnabled": true,  // Enable or disable audit logging
    "HideErrors": true, // Hide error information in audit logs
    "IsEnabledForAnonymousUsers": true, // Enable audit logging for anonymous users
    "IsEnabledForGetRequests": false,   // Enable audit logging for GET requests
    "ApplicationName": null  // Application name
  }
}
```

## Basic Usage

1. Reference the module
2. Configure audit logging options
3. Implement an audit log storage provider (e.g., EntityFrameworkCore or Elasticsearch)

## Advanced Features

### Ignoring Specific Types

By default, the module ignores audit logs for the following types:
* CancellationToken
* CancellationTokenSource

You can add more types to ignore through configuration:

```csharp
Configure<AbpAuditingOptions>(options =>
{
    options.IgnoredTypes.AddIfNotContains(typeof(YourType));
});
```
