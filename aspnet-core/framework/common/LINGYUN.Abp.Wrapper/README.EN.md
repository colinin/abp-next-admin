# LINGYUN.Abp.Wrapper

A wrapper module for unifying API response results and exception handling.

## Features

* Unified response result wrapping
* Flexible exception handling mechanism
* Support for multiple ignore strategies
* Configurable empty result handling
* Custom exception handlers

## Installation

```bash
dotnet add package LINGYUN.Abp.Wrapper
```

## Configuration

```csharp
[DependsOn(typeof(AbpWrapperModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpWrapperOptions>(options =>
        {
            // Enable wrapper
            options.IsEnabled = true;
            
            // Custom error code for unhandled exceptions
            options.CodeWithUnhandled = "500";
            
            // Ignore specific URL prefixes
            options.IgnorePrefixUrls.Add("/api/health");
            
            // Add custom exception handler
            options.AddHandler<CustomException>(new CustomExceptionHandler());
        });
    }
}
```

## Configuration Options

* `AbpWrapperOptions.IsEnabled` - Whether to wrap return results, default: false
* `AbpWrapperOptions.CodeWithUnhandled` - Error code for unhandled exceptions, default: 500
* `AbpWrapperOptions.CodeWithSuccess` - Success code for successful operations, default: 0
* `AbpWrapperOptions.ErrorWithEmptyResult` - Whether to return error message when resource is empty, default: false
* `AbpWrapperOptions.HttpStatusCode` - Http response code after wrapping, default: 200
* `AbpWrapperOptions.CodeWithEmptyResult` - Error code when returning empty object, default: 404
* `AbpWrapperOptions.MessageWithEmptyResult` - Error message when returning empty object, default: Not Found

* `AbpWrapperOptions.IgnorePrefixUrls` - Specify which URL prefixes to ignore
* `AbpWrapperOptions.IgnoreNamespaces` - Specify which namespaces to ignore
* `AbpWrapperOptions.IgnoreControllers` - Specify which controllers to ignore
* `AbpWrapperOptions.IgnoreReturnTypes` - Specify which return types to ignore
* `AbpWrapperOptions.IgnoreExceptions` - Specify which exception types to ignore
* `AbpWrapperOptions.IgnoredInterfaces` - Specify which interfaces to ignore (by default, implements **IWrapDisabled** interface will not be processed)

## Usage Examples

### 1. Basic Usage

```csharp
public class TestController : AbpController
{
    public async Task<WrapResult<string>> GetAsync()
    {
        return new WrapResult<string>("0", "Hello World");
    }
}
```

### 2. Ignore Wrapping

```csharp
[IgnoreWrapResult]
public class HealthController : AbpController
{
    public async Task<string> GetAsync()
    {
        return "OK";
    }
}
```

### 3. Custom Exception Handler

```csharp
public class CustomExceptionHandler : IExceptionWrapHandler
{
    public void Wrap(ExceptionWrapContext context)
    {
        context.WithCode("CUSTOM_ERROR")
               .WithMessage("Custom exception occurred")
               .WithDetails(context.Exception.Message);
    }
}
```

## Links

* [中文文档](./README.md)
