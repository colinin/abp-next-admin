# LINGYUN.Abp.OpenIddict.Application.Contracts

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Application.Contracts%2FLINGYUN.Abp.OpenIddict.Application.Contracts.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Application.Contracts.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Application.Contracts)

## Introduction

`LINGYUN.Abp.OpenIddict.Application.Contracts` is the contract layer for OpenIddict application services, defining the interfaces, DTOs, and permissions required for OpenIddict management.

[简体中文](./README.md)

## Features

* Define OpenIddict Application Service Interfaces
  * IOpenIddictApplicationAppService
  * IOpenIddictAuthorizationAppService
  * IOpenIddictTokenAppService
  * IOpenIddictScopeAppService

* Provide Standardized DTOs
  * OpenIddictApplicationDto
  * OpenIddictAuthorizationDto
  * OpenIddictTokenDto
  * OpenIddictScopeDto
  * And corresponding Create and Update DTOs

* Permission Definitions
  * OpenIddict.Applications
  * OpenIddict.Authorizations
  * OpenIddict.Tokens
  * OpenIddict.Scopes

* Multilingual Support
  * Built-in Chinese and English localization resources
  * Support for custom language extensions

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Application.Contracts
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictApplicationContractsModule))]` to your module class.

2. Inject and use the corresponding application service interfaces:

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

## Permissions

The module defines the following permissions:

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

## Localization

The module supports multiple languages with built-in support for:

* English (en)
* Simplified Chinese (zh-Hans)

You can extend new languages as follows:

```csharp
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<AbpOpenIddictResource>()
        .AddVirtualJson("/YourPath/Localization/Resources");
});
```
