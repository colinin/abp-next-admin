# LINGYUN.Abp.OpenIddict.HttpApi.Client

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.HttpApi.Client%2FLINGYUN.Abp.OpenIddict.HttpApi.Client.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.HttpApi.Client.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.HttpApi.Client)

## Introduction

`LINGYUN.Abp.OpenIddict.HttpApi.Client` is the HTTP API client module for OpenIddict, providing client proxies for remote OpenIddict HTTP API calls.

[简体中文](./README.md)

## Features

* HTTP API Client Proxies
  * Automatic HTTP client proxy generation
  * Support for remote service calls
  * Integration with ABP dynamic HTTP client proxies

* Remote Service Configuration
  * Support for configuring remote service URLs
  * Support for configuring authentication methods
  * Support for configuring request headers

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.HttpApi.Client
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictHttpApiClientModule))]` to your module class.

2. Configure remote services:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();
    
    Configure<AbpRemoteServiceOptions>(options =>
    {
        options.RemoteServices.Default = new RemoteServiceConfiguration
        {
            BaseUrl = configuration["RemoteServices:Default:BaseUrl"]
        };
    });
}
```

3. Usage example:

```csharp
// Inject client proxy
private readonly IOpenIddictApplicationAppService _applicationService;

public YourService(IOpenIddictApplicationAppService applicationService)
{
    _applicationService = applicationService;
}

// Call remote service
var input = new OpenIddictApplicationCreateDto
{
    ClientId = "your-client-id",
    DisplayName = "Your App",
    // ... other properties
};
var result = await _applicationService.CreateAsync(input);
```

## Configuration

* RemoteServices
  * Default:BaseUrl - Default remote service URL
  * OpenIddict:BaseUrl - OpenIddict remote service URL

## Notes

* Correct remote service URLs need to be configured
* If remote services require authentication, corresponding authentication information needs to be configured
* HTTPS is recommended in production environments
* Client proxies automatically handle authentication token transmission
