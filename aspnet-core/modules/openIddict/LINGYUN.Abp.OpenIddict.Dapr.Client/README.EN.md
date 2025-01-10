# LINGYUN.Abp.OpenIddict.Dapr.Client

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Dapr.Client%2FLINGYUN.Abp.OpenIddict.Dapr.Client.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Dapr.Client.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Dapr.Client)

## Introduction

`LINGYUN.Abp.OpenIddict.Dapr.Client` is a Dapr-based OpenIddict client module that provides functionality to call OpenIddict remote services using the Dapr service invocation building block.

[简体中文](./README.md)

## Features

* Dapr Service Invocation Integration
  * Automatic registration of Dapr client proxies for OpenIddict application contracts
  * Support accessing OpenIddict remote services via Dapr service invocation
  * Support service-to-service communication in distributed systems

* Remote Service Support
  * Support all services defined in OpenIddict application contracts
  * Support application management
  * Support authorization management
  * Support scope management
  * Support token management

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Dapr.Client
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictDaprClientModule))]` to your module class.

2. Configure Dapr service invocation:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpDaprClientOptions>(options =>
    {
        options.ApplicationServices.Configure(config =>
        {
            // Configure the AppId for OpenIddict service
            config.AppId = "openiddict-service";
        });
    });
}
```

3. Usage example:

```csharp
public class MyService
{
    private readonly IOpenIddictApplicationAppService _applicationAppService;

    public MyService(IOpenIddictApplicationAppService applicationAppService)
    {
        _applicationAppService = applicationAppService;
    }

    public async Task DoSomethingAsync()
    {
        // Access OpenIddict application service via Dapr service invocation
        var applications = await _applicationAppService.GetListAsync(
            new GetApplicationsInput());
    }
}
```

## Configuration

* AppId
  * Application identifier for the OpenIddict service
  * Must match the app ID in Dapr component configuration

* RemoteServiceName
  * Name of the OpenIddict remote service
  * Default value is "OpenIddict"

## Notes

* Ensure Dapr Sidecar is properly configured and running
* Ensure OpenIddict service is registered in Dapr
* Recommended to configure service-to-service authentication in production
* Recommended to configure retry policies for service invocation
* Recommended to configure service discovery mechanism
