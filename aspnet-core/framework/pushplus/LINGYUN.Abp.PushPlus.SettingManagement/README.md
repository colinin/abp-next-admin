# LINGYUN.Abp.PushPlus.SettingManagement

PushPlus 设置管理模块，提供 PushPlus 配置的管理界面和API接口。

## 功能特性

* 提供 PushPlus 配置管理接口
* 支持全局配置管理
* 支持租户配置管理
* 支持多语言
* 集成 ABP 设置管理模块

## 安装

```bash
dotnet add package LINGYUN.Abp.PushPlus.SettingManagement
```

## 模块引用

```csharp
[DependsOn(typeof(AbpPushPlusSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API接口

### 获取全局配置

```http
GET /api/setting-management/push-plus/by-global
```

响应示例：
```json
{
  "name": "PushPlus",
  "displayName": "PushPlus配置",
  "settings": [
    {
      "name": "PushPlus.Security.Token",
      "value": "your-token",
      "displayName": "Token",
      "description": "PushPlus平台获取的Token"
    },
    {
      "name": "PushPlus.Security.SecretKey",
      "value": "your-secret",
      "displayName": "密钥",
      "description": "PushPlus平台获取的密钥"
    }
  ]
}
```

### 获取租户配置

```http
GET /api/setting-management/push-plus/by-current-tenant
```

响应格式同全局配置。

## 权限

* Abp.PushPlus.ManageSetting - 管理PushPlus配置的权限

## 本地化

模块支持以下语言：
* 中文简体
* 英文

## 更多信息

* [LINGYUN.Abp.PushPlus](../LINGYUN.Abp.PushPlus/README.md)
* [ABP设置管理](https://docs.abp.io/en/abp/latest/Settings)
