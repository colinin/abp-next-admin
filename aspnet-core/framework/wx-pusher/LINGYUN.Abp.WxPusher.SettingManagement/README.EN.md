# LINGYUN.Abp.WxPusher.SettingManagement

WxPusher setting management module, providing management functionality for WxPusher-related settings.

[简体中文](./README.md)

## Features

* WxPusher settings management
  * AppToken management
  * Security settings management
* Permission control
  * Based on ABP permission system
  * Fine-grained settings management permissions
* Multi-tenant support
  * Global settings support
  * Tenant-level settings support
* Localization support
  * Multi-language interface
  * Localized setting descriptions

## Installation

```bash
dotnet add package LINGYUN.Abp.WxPusher.SettingManagement
```

## Module Reference

```csharp
[DependsOn(typeof(AbpWxPusherSettingManagementModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Permission Configuration

### 1. Permission Definitions

* `WxPusher`: WxPusher permission group
* `WxPusher.ManageSetting`: Permission to manage WxPusher settings

### 2. Authorization Configuration

```csharp
public class YourAuthorizationProvider : AuthorizationProvider
{
    public override void Define(IAuthorizationDefinitionContext context)
    {
        var wxPusher = context.GetPermissionOrNull(WxPusherSettingPermissionNames.GroupName)
            ?? context.AddGroup(WxPusherSettingPermissionNames.GroupName);

        wxPusher.AddPermission(WxPusherSettingPermissionNames.ManageSetting);
    }
}
```

## Usage

### 1. Getting Settings

```csharp
public class YourService
{
    private readonly IWxPusherSettingAppService _wxPusherSettingAppService;

    public YourService(IWxPusherSettingAppService wxPusherSettingAppService)
    {
        _wxPusherSettingAppService = wxPusherSettingAppService;
    }

    public async Task GetSettingsAsync()
    {
        // Get global settings
        var globalSettings = await _wxPusherSettingAppService.GetAllForGlobalAsync();

        // Get current tenant settings
        var tenantSettings = await _wxPusherSettingAppService.GetAllForCurrentTenantAsync();
    }
}
```

### 2. API Endpoints

* `GET /api/setting-management/wxpusher/by-global`: Get global settings
* `GET /api/setting-management/wxpusher/by-current-tenant`: Get current tenant settings

## Important Notes

1. Proper permission configuration is required to manage settings.
2. Sensitive information like AppToken needs to be properly secured.
3. Pay attention to the scope of settings in multi-tenant environments.

## Source Code

[LINGYUN.Abp.WxPusher.SettingManagement](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/wx-pusher/LINGYUN.Abp.WxPusher.SettingManagement)
