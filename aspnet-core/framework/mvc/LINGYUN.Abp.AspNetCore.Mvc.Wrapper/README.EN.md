# LINGYUN.Abp.AspNetCore.Mvc.Wrapper

MVC wrapper implementation module for unified wrapping of ASP.NET Core MVC response results.

[简体中文](./README.md)

## Features

* Automatic wrapping of MVC response results
* Support for custom wrapping rules
* Support for exception result wrapping
* Support for localized error messages
* Support for API documentation wrapping description
* Support for ignoring specific controllers, namespaces, and return types

## Installation

```bash
dotnet add package LINGYUN.Abp.AspNetCore.Mvc.Wrapper
```

## Configuration

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcWrapperModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpWrapperOptions>(options =>
        {
            // Enable wrapper
            options.IsEnabled = true;
            
            // Ignore specific return types
            options.IgnoreReturnTypes.Add<IRemoteStreamContent>();
            
            // Ignore specific controllers
            options.IgnoreControllers.Add<AbpApiDefinitionController>();
            
            // Ignore specific URL prefixes
            options.IgnorePrefixUrls.Add("/connect");
            
            // Custom empty result message
            options.MessageWithEmptyResult = (serviceProvider) =>
            {
                var localizer = serviceProvider.GetRequiredService<IStringLocalizer<AbpMvcWrapperResource>>();
                return localizer["Wrapper:NotFound"];
            };
        });
    }
}
```

## Configuration Options

* `IsEnabled`: Whether to enable the wrapper
* `IsWrapUnauthorizedEnabled`: Whether to wrap unauthorized responses
* `HttpStatusCode`: HTTP status code for wrapped responses
* `IgnoreReturnTypes`: List of return types to ignore wrapping
* `IgnoreControllers`: List of controllers to ignore wrapping
* `IgnoreNamespaces`: List of namespaces to ignore wrapping
* `IgnorePrefixUrls`: List of URL prefixes to ignore wrapping
* `IgnoreExceptions`: List of exception types to ignore wrapping
* `ErrorWithEmptyResult`: Whether to return error message for empty results
* `CodeWithEmptyResult`: Error code for empty results
* `MessageWithEmptyResult`: Error message for empty results
* `CodeWithSuccess`: Code for successful responses

## Advanced Usage

### 1. Using Attributes to Ignore Wrapping

```csharp
[IgnoreWrapResult]
public class TestController : AbpController
{
    // All actions in this controller will not be wrapped
}

public class TestController : AbpController
{
    [IgnoreWrapResult]
    public IActionResult Test()
    {
        // This action will not be wrapped
    }
}
```

### 2. Controlling Wrapping via Request Headers

* Add `_AbpDontWrapResult` header to disable wrapping
* Add `_AbpWrapResult` header to force enable wrapping

## Source Code

[LINGYUN.Abp.AspNetCore.Mvc.Wrapper](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/mvc/LINGYUN.Abp.AspNetCore.Mvc.Wrapper)
