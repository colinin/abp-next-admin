# LINGYUN.Abp.Auditing.HttpApi

HTTP API module for audit logging, providing REST API interfaces for audit logging functionality.

[简体中文](./README.md)

## Features

* Audit log REST API interfaces
* Security log REST API interfaces
* Support for API versioning
* Integration with Swagger documentation

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuditingHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Endpoints

### Audit Logs

* GET /api/audit-logging/audit-logs/{id} - Get a specific audit log
* GET /api/audit-logging/audit-logs - Get a list of audit logs
* DELETE /api/audit-logging/audit-logs/{id} - Delete a specific audit log
* DELETE /api/audit-logging/audit-logs/batch - Batch delete audit logs

### Security Logs

* GET /api/audit-logging/security-logs/{id} - Get a specific security log
* GET /api/audit-logging/security-logs - Get a list of security logs
* DELETE /api/audit-logging/security-logs/{id} - Delete a specific security log
* DELETE /api/audit-logging/security-logs/batch - Batch delete security logs

## Basic Usage

1. Reference the module
2. Configure permissions (if needed)
3. Call the appropriate API endpoints using an HTTP client

## API Documentation

After starting the application, you can view the complete API documentation through:
* /swagger - Swagger UI interface
* /swagger/v1/swagger.json - OpenAPI specification document
