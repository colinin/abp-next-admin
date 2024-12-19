# LINGYUN.Abp.SettingManagement.Application

## 模块说明

设置管理应用服务模块，实现设置管理相关的业务逻辑。

### 基础模块

* LINGYUN.Abp.SettingManagement.Application.Contracts
* Volo.Abp.SettingManagement.Application
* Volo.Abp.Ddd.Application

### 功能定义

* 提供设置管理的应用服务实现
  * SettingAppService - 通用设置管理服务实现
  * UserSettingAppService - 用户设置管理服务实现
  * SettingDefinitionAppService - 设置定义管理服务实现
* 实现以下应用服务接口
  * ISettingAppService
  * IUserSettingAppService
  * ISettingDefinitionAppService
* 提供设置缓存管理
  * DynamicSettingDefinitionStoreCacheInvalidator - 动态设置定义缓存失效处理

### 应用服务

* SettingAppService
  * GetAllForGlobalAsync - 获取全局设置
  * GetAllForTenantAsync - 获取租户设置
  * GetAllForUserAsync - 获取用户设置
  * GetAllGroupsAsync - 获取所有设置组
  * UpdateAsync - 更新设置
* UserSettingAppService
  * GetAsync - 获取用户设置
  * UpdateAsync - 更新用户设置
  * DeleteAsync - 删除用户设置
* SettingDefinitionAppService
  * GetAsync - 获取设置定义
  * GetListAsync - 获取设置定义列表
  * CreateAsync - 创建设置定义
  * UpdateAsync - 更新设置定义
  * DeleteAsync - 删除设置定义

### 错误代码

* SettingManagement:010001 - 设置定义名称已存在
* SettingManagement:010002 - 设置定义不存在
* SettingManagement:010003 - 设置定义为静态，不允许修改
* SettingManagement:010004 - 设置定义为静态，不允许删除

### 如何使用

1. 添加 `AbpSettingManagementApplicationModule` 依赖

```csharp
[DependsOn(typeof(AbpSettingManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 注入并使用设置服务

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
        // 获取全局设置
        var settings = await _settingAppService.GetAllForGlobalAsync();

        // 更新用户设置
        await _userSettingAppService.UpdateAsync(
            "SettingName",
            "NewValue");
    }
}
```

[查看英文](README.EN.md)
