# LINGYUN.Abp.WxPusher.SettingManagement

WxPusher设置管理模块，提供WxPusher相关设置的管理功能。

[English](./README.EN.md)

## 功能特性

* 支持WxPusher设置管理
  * AppToken管理
  * 安全设置管理
* 权限控制
  * 基于ABP权限系统
  * 细粒度的设置管理权限
* 多租户支持
  * 支持全局设置
  * 支持租户级设置
* 本地化支持
  * 支持多语言界面
  * 支持本地化设置描述

## 安装

```bash
dotnet add package LINGYUN.Abp.WxPusher.SettingManagement
```

## 模块引用

```csharp
[DependsOn(typeof(AbpWxPusherSettingManagementModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 权限配置

### 1. 权限定义

* `WxPusher`: WxPusher权限组
* `WxPusher.ManageSetting`: 管理WxPusher设置的权限

### 2. 授权配置

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

## 使用方式

### 1. 获取设置

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
        // 获取全局设置
        var globalSettings = await _wxPusherSettingAppService.GetAllForGlobalAsync();

        // 获取当前租户设置
        var tenantSettings = await _wxPusherSettingAppService.GetAllForCurrentTenantAsync();
    }
}
```

### 2. API接口

* `GET /api/setting-management/wxpusher/by-global`: 获取全局设置
* `GET /api/setting-management/wxpusher/by-current-tenant`: 获取当前租户设置

## 注意事项

1. 需要正确配置权限才能管理设置。
2. AppToken等敏感信息需要妥善保管。
3. 多租户环境下需要注意设置的作用范围。

## 源码位置

[LINGYUN.Abp.WxPusher.SettingManagement](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/wx-pusher/LINGYUN.Abp.WxPusher.SettingManagement)
