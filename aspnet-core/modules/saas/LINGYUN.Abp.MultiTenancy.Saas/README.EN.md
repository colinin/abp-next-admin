# LINGYUN.Abp.MultiTenancy.Saas

Multi-tenant SaaS management module, providing tenant management, version management and other features.

## Features

* Tenant Management: Create, edit, delete tenants, manage tenant connection strings
* Edition Management: Create, edit, delete editions, manage edition features
* Tenant Expiration Management: Support tenant expiration time setting, expiration warning, expired resource recycling
* Tenant Feature Management: Support assigning feature permissions to tenants

## Configuration

### Module Configuration

```json
{
  "AbpSaas": {
    "Tenants": {
      "RecycleStrategy": "1",           // Resource recycling strategy: 0-Reserve, 1-Recycle
      "ExpirationReminderDays": "15",   // Expiration warning days, range 1-30 days
      "ExpiredRecoveryTime": "15"       // Expired recovery time, range 1-30 days
    }
  }
}
```

### Permission Configuration

* AbpSaas.Editions
  * AbpSaas.Editions.Create: Create edition
  * AbpSaas.Editions.Update: Update edition
  * AbpSaas.Editions.Delete: Delete edition
  * AbpSaas.Editions.ManageFeatures: Manage edition features

* AbpSaas.Tenants
  * AbpSaas.Tenants.Create: Create tenant
  * AbpSaas.Tenants.Update: Update tenant
  * AbpSaas.Tenants.Delete: Delete tenant
  * AbpSaas.Tenants.ManageFeatures: Manage tenant features
  * AbpSaas.Tenants.ManageConnectionStrings: Manage tenant connection strings

## More

[简体中文](README.md)
