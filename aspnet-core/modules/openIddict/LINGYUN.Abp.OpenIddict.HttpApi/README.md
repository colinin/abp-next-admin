# LINGYUN.Abp.OpenIddict.HttpApi

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.HttpApi%2FLINGYUN.Abp.OpenIddict.HttpApi.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.HttpApi.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.HttpApi)

## 简介

`LINGYUN.Abp.OpenIddict.HttpApi` 是 OpenIddict 的 HTTP API 模块，提供了 OpenIddict 相关功能的 RESTful API 接口。

[English](./README.EN.md)

## 功能特性

* OpenIddict 应用程序管理
  * 创建、更新、删除应用程序
  * 查询应用程序列表
  * 获取应用程序详情

* OpenIddict 授权管理
  * 查询授权列表
  * 获取授权详情
  * 删除授权记录

* OpenIddict 令牌管理
  * 查询令牌列表
  * 获取令牌详情
  * 删除令牌记录

* OpenIddict 作用域管理
  * 创建、更新、删除作用域
  * 查询作用域列表
  * 获取作用域详情

* 多语言支持
  * 集成 ABP 本地化框架
  * 支持自定义本地化资源

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.HttpApi
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictHttpApiModule))]` 到你的模块类。

2. 配置权限:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpPermissionOptions>(options =>
    {
        options.ValueProviders.Add<OpenIddictPermissionValueProvider>();
    });
}
```

3. API 接口使用示例:

```csharp
// 注入服务
private readonly IOpenIddictApplicationAppService _applicationService;

public YourService(IOpenIddictApplicationAppService applicationService)
{
    _applicationService = applicationService;
}

// 创建应用程序
var input = new OpenIddictApplicationCreateDto
{
    ClientId = "your-client-id",
    DisplayName = "Your App",
    // ... 其他属性
};
var result = await _applicationService.CreateAsync(input);

// 查询应用程序列表
var query = new OpenIddictApplicationGetListInput
{
    MaxResultCount = 10,
    SkipCount = 0,
    Filter = "search-term"
};
var list = await _applicationService.GetListAsync(query);
```

## 权限

* OpenIddict.Applications
  * OpenIddict.Applications.Create
  * OpenIddict.Applications.Update
  * OpenIddict.Applications.Delete
  * OpenIddict.Applications.ManagePermissions

* OpenIddict.Scopes
  * OpenIddict.Scopes.Create
  * OpenIddict.Scopes.Update
  * OpenIddict.Scopes.Delete
  * OpenIddict.Scopes.ManagePermissions

* OpenIddict.Authorizations
  * OpenIddict.Authorizations.Delete
  * OpenIddict.Authorizations.ManagePermissions

* OpenIddict.Tokens
  * OpenIddict.Tokens.Delete
  * OpenIddict.Tokens.ManagePermissions

## 注意事项

* 所有 API 接口都需要相应的权限才能访问
* 删除应用程序会同时删除相关的授权和令牌
* API 接口支持多租户场景
* 建议在生产环境中启用 API 认证和授权
