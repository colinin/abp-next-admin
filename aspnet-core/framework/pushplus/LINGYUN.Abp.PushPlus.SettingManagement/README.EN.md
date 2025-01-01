# LINGYUN.Abp.PushPlus.SettingManagement

PushPlus settings management module, providing management interface and API endpoints for PushPlus configuration.

## Features

* Provides PushPlus configuration management API
* Supports global configuration management
* Supports tenant configuration management
* Supports multilingual
* Integrates with ABP settings management module

## Installation

```bash
dotnet add package LINGYUN.Abp.PushPlus.SettingManagement
```

## Module Dependencies

```csharp
[DependsOn(typeof(AbpPushPlusSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Endpoints

### Get Global Configuration

```http
GET /api/setting-management/push-plus/by-global
```

Response example:
```json
{
  "name": "PushPlus",
  "displayName": "PushPlus Configuration",
  "settings": [
    {
      "name": "PushPlus.Security.Token",
      "value": "your-token",
      "displayName": "Token",
      "description": "Token obtained from PushPlus platform"
    },
    {
      "name": "PushPlus.Security.SecretKey",
      "value": "your-secret",
      "displayName": "Secret Key",
      "description": "Secret key obtained from PushPlus platform"
    }
  ]
}
```

### Get Tenant Configuration

```http
GET /api/setting-management/push-plus/by-current-tenant
```

Response format is the same as global configuration.

## Permissions

* Abp.PushPlus.ManageSetting - Permission to manage PushPlus configuration

## Localization

The module supports the following languages:
* Simplified Chinese
* English

## More Information

* [LINGYUN.Abp.PushPlus](../LINGYUN.Abp.PushPlus/README.md)
* [ABP Settings Management](https://docs.abp.io/en/abp/latest/Settings)
