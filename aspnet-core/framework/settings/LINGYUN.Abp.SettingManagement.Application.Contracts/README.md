# LINGYUN.Abp.SettingManagement.Application.Contracts

## 模块说明

设置管理应用服务契约模块，提供设置管理相关的接口定义和DTO。

### 基础模块  

* Volo.Abp.SettingManagement.Application.Contracts
* Volo.Abp.Ddd.Application.Contracts

### 功能定义  

* 提供设置管理的应用服务接口定义
  * ISettingAppService - 通用设置管理服务
  * IUserSettingAppService - 用户设置管理服务
  * IReadonlySettingAppService - 只读设置服务
* 提供设置相关的DTO定义
  * SettingGroupDto - 设置组DTO
  * SettingDto - 设置DTO
  * SettingDetailsDto - 设置详情DTO
  * UpdateSettingDto - 更新设置DTO
* 提供设置管理相关的权限定义

### 权限定义

* SettingManagement.ManageHostFeatures - 管理主机功能
* SettingManagement.ManageFeatures - 管理功能
* SettingManagement.Settings - 设置管理
* SettingManagement.Settings.Update - 更新设置
* SettingManagement.Settings.ManageGroup - 管理设置组

### 配置定义  

* SettingManagementMergeOptions
  * EnableCustomize - 是否启用自定义设置
  * EnableHost - 是否启用主机设置
  * EnableTenant - 是否启用租户设置
  * EnableUser - 是否启用用户设置

### 如何使用

1. 添加 `AbpSettingManagementApplicationContractsModule` 依赖

```csharp
[DependsOn(typeof(AbpSettingManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 注入并使用设置服务

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
        // 获取设置组
        var groups = await _settingAppService.GetAllGroupsAsync();
        
        // 更新设置
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

[返回目录](../../../README.md)
