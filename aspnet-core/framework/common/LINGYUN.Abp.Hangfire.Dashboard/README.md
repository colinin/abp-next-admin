# LINGYUN.Abp.Hangfire.Dashboard

[English](README.EN.md) | 简体中文

## 1. 介绍

`LINGYUN.Abp.Hangfire.Dashboard` 是一个用于集成Hangfire仪表板的ABP模块，它提供了一个用户友好的Web界面来监控和管理Hangfire后台作业。该模块支持权限控制和认证，确保仪表板的安全访问。

## 2. 功能特性

* 集成Hangfire仪表板
* 基于ABP权限系统的访问控制
* 支持通过iframe加载仪表板
* 支持访问令牌认证
* 仪表板权限缓存机制

## 3. 安装

```bash
dotnet add package LINGYUN.Abp.Hangfire.Dashboard
```

## 4. 使用方法

1. 添加 `AbpHangfireDashboardModule` 到模块依赖中:

```csharp
[DependsOn(typeof(AbpHangfireDashboardModule))]
public class YourModule : AbpModule
{
}
```

2. 配置中间件:

```csharp
public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    var app = context.GetApplicationBuilder();
    
    // 添加Hangfire认证中间件
    app.UseHangfireAuthorication();
}
```

3. 配置仪表板选项:

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

## 5. 认证和授权

### 5.1 认证方式

模块支持以下认证方式：
* 通过URL参数传递访问令牌: `?access_token=your_token`
* 通过Cookie传递访问令牌
* 通过Authorization Header传递访问令牌

### 5.2 权限缓存

权限检查结果会被缓存5分钟，以提高性能。

## 6. 依赖项

* Volo.Abp.Authorization
* Volo.Abp.Hangfire
* Microsoft.Extensions.Caching.Memory

## 7. 文档和资源

* [Hangfire官方文档](https://docs.hangfire.io/)
* [ABP框架文档](https://docs.abp.io/)
