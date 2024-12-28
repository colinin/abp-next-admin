# LINGYUN.Abp.SettingManagement.HttpApi

## Module Description

Setting management HTTP API module, providing RESTful API interfaces for setting management.

### Base Modules

* LINGYUN.Abp.SettingManagement.Application.Contracts
* Volo.Abp.AspNetCore.Mvc

### Features

* Provides API controllers for setting management
  * SettingController - General setting management controller
  * UserSettingController - User setting management controller
  * SettingDefinitionController - Setting definition management controller

### API Endpoints

* /api/setting-management/settings
  * GET /by-global - Get global settings
  * GET /by-tenant - Get tenant settings
  * GET /by-user - Get user settings
  * GET /groups - Get all setting groups
  * PUT /{providerName}/{providerKey} - Update settings
* /api/setting-management/users
  * GET - Get user settings
  * PUT - Update user settings
  * DELETE - Delete user settings
* /api/setting-management/definitions
  * GET - Get setting definition list
  * POST - Create setting definition
  * PUT - Update setting definition
  * DELETE - Delete setting definition
  * GET /{name} - Get specific setting definition

### Permission Requirements

* SettingManagement.Settings
  * Update - Update settings
  * ManageGroup - Manage setting groups
* SettingManagement.ManageFeatures
  * ManageHostFeatures - Manage host features

### How to Use

1. Add `AbpSettingManagementHttpApiModule` dependency

```csharp
[DependsOn(typeof(AbpSettingManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use API endpoints

```csharp
public class YourService
{
    private readonly HttpClient _httpClient;

    public YourService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ManageSettingsAsync()
    {
        // Get global settings
        var response = await _httpClient.GetAsync("/api/setting-management/settings/by-global");
        var settings = await response.Content.ReadFromJsonAsync<ListResultDto<SettingGroupDto>>();

        // Update user settings
        await _httpClient.PutAsJsonAsync("/api/setting-management/users", new UpdateSettingDto
        {
            Name = "SettingName",
            Value = "NewValue"
        });
    }
}
```

[查看中文](README.md)
