# LINGYUN.Abp.OpenIddict.HttpApi

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.HttpApi%2FLINGYUN.Abp.OpenIddict.HttpApi.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.HttpApi.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.HttpApi)

## Introduction

`LINGYUN.Abp.OpenIddict.HttpApi` is the HTTP API module for OpenIddict, providing RESTful API interfaces for OpenIddict-related functionality.

[简体中文](./README.md)

## Features

* OpenIddict Application Management
  * Create, update, delete applications
  * Query application list
  * Get application details

* OpenIddict Authorization Management
  * Query authorization list
  * Get authorization details
  * Delete authorization records

* OpenIddict Token Management
  * Query token list
  * Get token details
  * Delete token records

* OpenIddict Scope Management
  * Create, update, delete scopes
  * Query scope list
  * Get scope details

* Localization Support
  * Integrated with ABP localization framework
  * Support for custom localization resources

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.HttpApi
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictHttpApiModule))]` to your module class.

2. Configure permissions:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpPermissionOptions>(options =>
    {
        options.ValueProviders.Add<OpenIddictPermissionValueProvider>();
    });
}
```

3. API usage example:

```csharp
// Inject service
private readonly IOpenIddictApplicationAppService _applicationService;

public YourService(IOpenIddictApplicationAppService applicationService)
{
    _applicationService = applicationService;
}

// Create application
var input = new OpenIddictApplicationCreateDto
{
    ClientId = "your-client-id",
    DisplayName = "Your App",
    // ... other properties
};
var result = await _applicationService.CreateAsync(input);

// Query application list
var query = new OpenIddictApplicationGetListInput
{
    MaxResultCount = 10,
    SkipCount = 0,
    Filter = "search-term"
};
var list = await _applicationService.GetListAsync(query);
```

## Permissions

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

## Notes

* All API endpoints require corresponding permissions to access
* Deleting an application will also delete related authorizations and tokens
* API endpoints support multi-tenant scenarios
* It is recommended to enable API authentication and authorization in production environments
