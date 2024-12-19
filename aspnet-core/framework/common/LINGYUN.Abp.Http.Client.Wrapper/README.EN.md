# LINGYUN.Abp.Http.Client.Wrapper

HTTP client wrapper module for automatically adding wrapper request headers in HTTP client requests.

[简体中文](./README.md)

## Features

* Automatic addition of wrapper request headers
* Integration with ABP HTTP client
* Support for global wrapper configuration

## Installation

```bash
dotnet add package LINGYUN.Abp.Http.Client.Wrapper
```

## Configuration

```csharp
[DependsOn(typeof(AbpHttpClientWrapperModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpWrapperOptions>(options =>
        {
            // Enable wrapper
            options.IsEnabled = true;
        });
    }
}
```

## How It Works

When the wrapper is enabled (`AbpWrapperOptions.IsEnabled = true`), the module automatically adds the `_AbpWrapResult` header to all HTTP client requests.
When the wrapper is disabled (`AbpWrapperOptions.IsEnabled = false`), the module automatically adds the `_AbpDontWrapResult` header to all HTTP client requests.

This ensures that the HTTP client request results remain consistent with the server-side wrapper configuration.

## Source Code

[LINGYUN.Abp.Http.Client.Wrapper](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/common/LINGYUN.Abp.Http.Client.Wrapper)
