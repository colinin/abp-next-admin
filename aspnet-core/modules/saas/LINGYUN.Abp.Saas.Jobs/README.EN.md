# LINGYUN.Abp.Saas.Jobs

SaaS background job module, providing background job implementation for tenant resource monitoring and management.

## Features

* Tenant Usage Monitoring Job (TenantUsageMonitoringJob)
  * Monitor tenant resource usage
  * Handle tenant expiration warning
  * Handle expired tenant resource recycling

## Configuration

### Job Configuration

```json
{
  "Hangfire": {
    "TenantUsageMonitoring": {
      "CronExpression": "0 0 * * *",  // Run once per day
      "Queue": "default",              // Job queue
      "Enabled": true                  // Whether to enable
    }
  }
}
```

### Job Parameters

* Saas:AdminEmail: Administrator email address for receiving warning notifications
* Saas:TenantId: Tenant identifier

## More

[简体中文](README.md)
