# LINGYUN.Abp.OpenIddict.Application.Contracts

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Application.Contracts%2FLINGYUN.Abp.OpenIddict.Application.Contracts.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Application.Contracts.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Application.Contracts)

## 简介

`LINGYUN.Abp.OpenIddict.Application.Contracts` 是 OpenIddict 应用服务的契约层，定义了 OpenIddict 管理所需的接口、DTO和权限。

[English](./README.EN.md)

## 功能特性

* 定义 OpenIddict 应用服务接口
  * IOpenIddictApplicationAppService
  * IOpenIddictAuthorizationAppService
  * IOpenIddictTokenAppService
  * IOpenIddictScopeAppService

* 提供标准化的 DTO 对象
  * OpenIddictApplicationDto
  * OpenIddictAuthorizationDto
  * OpenIddictTokenDto
  * OpenIddictScopeDto
  * 以及相应的创建和更新 DTO

* 权限定义
  * OpenIddict.Applications
  * OpenIddict.Authorizations
  * OpenIddict.Tokens
  * OpenIddict.Scopes

* 多语言支持
  * 内置中文和英文本地化资源
  * 支持自定义语言扩展

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Application.Contracts
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictApplicationContractsModule))]` 到你的模块类。

2. 注入并使用相应的应用服务接口：

```csharp
public class YourService
{
    private readonly IOpenIddictApplicationAppService _applicationAppService;
    
    public YourService(IOpenIddictApplicationAppService applicationAppService)
    {
        _applicationAppService = applicationAppService;
    }
    
    public async Task DoSomethingAsync()
    {
        var applications = await _applicationAppService.GetListAsync(
            new OpenIddictApplicationGetListInput());
        // ...
    }
}
```

## 权限

模块定义了以下权限：

* OpenIddict.Applications
  * OpenIddict.Applications.Create
  * OpenIddict.Applications.Update
  * OpenIddict.Applications.Delete
  * OpenIddict.Applications.ManagePermissions
  * OpenIddict.Applications.ManageSecret
* OpenIddict.Authorizations
  * OpenIddict.Authorizations.Delete
* OpenIddict.Scopes
  * OpenIddict.Scopes.Create
  * OpenIddict.Scopes.Update
  * OpenIddict.Scopes.Delete
* OpenIddict.Tokens
  * OpenIddict.Tokens.Delete

## 本地化

模块支持多语言，内置了以下语言：

* 英文 (en)
* 简体中文 (zh-Hans)

可以通过以下方式扩展新的语言：

```csharp
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<AbpOpenIddictResource>()
        .AddVirtualJson("/YourPath/Localization/Resources");
});
```
