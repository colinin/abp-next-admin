# LINGYUN.Abp.SettingManagement.Application

## Module Description

Setting management application service module, implementing business logic for setting management.

### Base Modules

* LINGYUN.Abp.SettingManagement.Application.Contracts
* Volo.Abp.SettingManagement.Application
* Volo.Abp.Ddd.Application

### Features

* Provides implementation of setting management application services
  * SettingAppService - General setting management service implementation
  * UserSettingAppService - User setting management service implementation
  * SettingDefinitionAppService - Setting definition management service implementation
* Implements the following application service interfaces
  * ISettingAppService
  * IUserSettingAppService
  * ISettingDefinitionAppService
* Provides setting cache management
  * DynamicSettingDefinitionStoreCacheInvalidator - Dynamic setting definition cache invalidation handler

### Application Services

* SettingAppService
  * GetAllForGlobalAsync - Get global settings
  * GetAllForTenantAsync - Get tenant settings
  * GetAllForUserAsync - Get user settings
  * GetAllGroupsAsync - Get all setting groups
  * UpdateAsync - Update settings
* UserSettingAppService
  * GetAsync - Get user settings
  * UpdateAsync - Update user settings
  * DeleteAsync - Delete user settings
* SettingDefinitionAppService
  * GetAsync - Get setting definition
  * GetListAsync - Get setting definition list
  * CreateAsync - Create setting definition
  * UpdateAsync - Update setting definition
  * DeleteAsync - Delete setting definition

### Error Codes

* SettingManagement:010001 - Setting definition name already exists
* SettingManagement:010002 - Setting definition does not exist
* SettingManagement:010003 - Setting definition is static, modification not allowed
* SettingManagement:010004 - Setting definition is static, deletion not allowed

### How to Use

1. Add `AbpSettingManagementApplicationModule` dependency

```csharp
[DependsOn(typeof(AbpSettingManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Inject and use setting services

```csharp
public class YourService
{
    private readonly ISettingAppService _settingAppService;
    private readonly IUserSettingAppService _userSettingAppService;

    public YourService(
        ISettingAppService settingAppService,
        IUserSettingAppService userSettingAppService)
    {
        _settingAppService = settingAppService;
        _userSettingAppService = userSettingAppService;
    }

    public async Task ManageSettingsAsync()
    {
        // Get global settings
        var settings = await _settingAppService.GetAllForGlobalAsync();

        // Update user settings
        await _userSettingAppService.UpdateAsync(
            "SettingName",
            "NewValue");
    }
}
```

[查看中文](README.md)
