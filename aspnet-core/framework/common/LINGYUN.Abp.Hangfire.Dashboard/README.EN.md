# LINGYUN.Abp.Hangfire.Dashboard

English | [简体中文](README.md)

## 1. Introduction

`LINGYUN.Abp.Hangfire.Dashboard` is an ABP module for integrating the Hangfire dashboard, providing a user-friendly web interface for monitoring and managing Hangfire background jobs. This module supports permission control and authentication to ensure secure access to the dashboard.

## 2. Features

* Integration with Hangfire dashboard
* Access control based on ABP permission system
* Support for loading dashboard in iframe
* Access token authentication support
* Dashboard permission caching mechanism

## 3. Installation

```bash
dotnet add package LINGYUN.Abp.Hangfire.Dashboard
```

## 4. Usage

1. Add `AbpHangfireDashboardModule` to your module dependencies:

```csharp
[DependsOn(typeof(AbpHangfireDashboardModule))]
public class YourModule : AbpModule
{
}
```

2. Configure middleware:

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    var app = context.GetApplicationBuilder();
    
    // Add Hangfire authentication middleware
    app.UseHangfireAuthorication();
}
```

3. Configure dashboard options:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<DashboardOptions>(options =>
    {
        options.Authorization = new[]
        {
            new DashboardAuthorizationFilter("YourPermissionName")
        };
    });
}
```

## 5. Authentication and Authorization

### 5.1 Authentication Methods

The module supports the following authentication methods:
* Pass access token via URL parameter: `?access_token=your_token`
* Pass access token via Cookie
* Pass access token via Authorization Header

### 5.2 Permission Caching

Permission check results are cached for 5 minutes to improve performance.

## 6. Dependencies

* Volo.Abp.Authorization
* Volo.Abp.Hangfire
* Microsoft.Extensions.Caching.Memory

## 7. Documentation and Resources

* [Hangfire Official Documentation](https://docs.hangfire.io/)
* [ABP Framework Documentation](https://docs.abp.io/)
