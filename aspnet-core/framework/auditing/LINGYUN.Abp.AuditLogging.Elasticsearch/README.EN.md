# LINGYUN.Abp.AuditLogging.Elasticsearch

[简体中文](./README.md) | English

Elasticsearch implementation for the audit logging module.

## Features

* ElasticsearchAuditLogManager - Implements IAuditLogManager, managing audit logs with Elasticsearch
* ElasticsearchSecurityLogManager - Implements ISecurityLogManager, managing security logs with Elasticsearch

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuditLoggingElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Options

* AbpAuditLoggingElasticsearchOptions.IndexPrefix - Index prefix, default is 'auditlogging'
* AbpAuditLoggingElasticsearchOptions.IndexSettings - Elasticsearch index settings

## Multi-tenancy Support

When integrated with the tenant module, the index will switch based on the tenant:
- For tenant-specific data: `{prefix}-{index}-{tenantId}`
- For host data: `{prefix}-{index}`

## appsettings.json

```json
{
  "AuditLogging": {
    "Elasticsearch": {
      "IndexPrefix": "auditlogging"
    }
  }
}
```

## Features

1. Audit Log Management
   - Store and retrieve audit logs in Elasticsearch
   - Support for entity change tracking
   - Flexible querying with various filters
   - Support for extra properties

2. Security Log Management
   - Store and retrieve security logs in Elasticsearch
   - Support for security-related events tracking
   - Comprehensive querying capabilities

3. Index Management
   - Automatic index initialization
   - Support for custom index settings
   - Multi-tenant index isolation
