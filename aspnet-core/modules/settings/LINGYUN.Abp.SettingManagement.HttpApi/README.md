# LINGYUN.Abp.SettingManagement.HttpApi

## 模块说明

设置管理 HTTP API 模块，提供设置管理的 RESTful API 接口。

### 基础模块

* LINGYUN.Abp.SettingManagement.Application.Contracts
* Volo.Abp.AspNetCore.Mvc

### 功能定义

* 提供设置管理的 API 控制器
  * SettingController - 通用设置管理控制器
  * UserSettingController - 用户设置管理控制器
  * SettingDefinitionController - 设置定义管理控制器

### API 接口

* /api/setting-management/settings
  * GET /by-global - 获取全局设置
  * GET /by-tenant - 获取租户设置
  * GET /by-user - 获取用户设置
  * GET /groups - 获取所有设置组
  * PUT /{providerName}/{providerKey} - 更新设置
* /api/setting-management/users
  * GET - 获取用户设置
  * PUT - 更新用户设置
  * DELETE - 删除用户设置
* /api/setting-management/definitions
  * GET - 获取设置定义列表
  * POST - 创建设置定义
  * PUT - 更新设置定义
  * DELETE - 删除设置定义
  * GET /{name} - 获取指定设置定义

### 权限要求

* SettingManagement.Settings
  * Update - 更新设置
  * ManageGroup - 管理设置组
* SettingManagement.ManageFeatures
  * ManageHostFeatures - 管理主机功能

### 如何使用

1. 添加 `AbpSettingManagementHttpApiModule` 依赖

```csharp
[DependsOn(typeof(AbpSettingManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用 API 接口

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
        // 获取全局设置
        var response = await _httpClient.GetAsync("/api/setting-management/settings/by-global");
        var settings = await response.Content.ReadFromJsonAsync<ListResultDto<SettingGroupDto>>();

        // 更新用户设置
        await _httpClient.PutAsJsonAsync("/api/setting-management/users", new UpdateSettingDto
        {
            Name = "SettingName",
            Value = "NewValue"
        });
    }
}
```

[查看英文](README.EN.md)
