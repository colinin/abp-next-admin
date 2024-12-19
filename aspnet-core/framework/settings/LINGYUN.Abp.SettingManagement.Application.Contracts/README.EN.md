# LINGYUN.Abp.SettingManagement.Application.Contracts

## Module Description

Setting management application service contracts module, providing interface definitions and DTOs for setting management.

### Base Modules  

* Volo.Abp.SettingManagement.Application.Contracts
* Volo.Abp.Ddd.Application.Contracts

### Features  

* Provides setting management application service interfaces
  * ISettingAppService - General setting management service
  * IUserSettingAppService - User setting management service
  * IReadonlySettingAppService - Read-only setting service
* Provides setting-related DTO definitions
  * SettingGroupDto - Setting group DTO
  * SettingDto - Setting DTO
  * SettingDetailsDto - Setting details DTO
  * UpdateSettingDto - Update setting DTO
* Provides setting management related permission definitions

### Permissions

* SettingManagement.ManageHostFeatures - Manage host features
* SettingManagement.ManageFeatures - Manage features
* SettingManagement.Settings - Setting management
* SettingManagement.Settings.Update - Update settings
* SettingManagement.Settings.ManageGroup - Manage setting groups

### Configuration  

* SettingManagementMergeOptions
  * EnableCustomize - Enable custom settings
  * EnableHost - Enable host settings
  * EnableTenant - Enable tenant settings
  * EnableUser - Enable user settings

### How to Use

1. Add `AbpSettingManagementApplicationContractsModule` dependency

```csharp
[DependsOn(typeof(AbpSettingManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Inject and use setting services

```csharp
public class YourService
{
    private readonly ISettingAppService _settingAppService;

    public YourService(ISettingAppService settingAppService)
    {
        _settingAppService = settingAppService;
    }

    public async Task ManageSettingsAsync()
    {
        // Get setting groups
        var groups = await _settingAppService.GetAllGroupsAsync();
        
        // Update settings
        await _settingAppService.UpdateAsync("GroupName", new UpdateSettingsDto
        {
            Settings = new List<UpdateSettingDto>
            {
                new UpdateSettingDto
                {
                    Name = "SettingName",
                    Value = "NewValue"
                }
            }
        });
    }
}
```

[Back to TOC](../../../README.md)
