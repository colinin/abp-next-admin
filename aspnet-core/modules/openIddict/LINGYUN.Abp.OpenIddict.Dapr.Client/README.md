# LINGYUN.Abp.OpenIddict.Dapr.Client

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Dapr.Client%2FLINGYUN.Abp.OpenIddict.Dapr.Client.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Dapr.Client.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Dapr.Client)

## 介绍

`LINGYUN.Abp.OpenIddict.Dapr.Client` 是一个基于 Dapr 的 OpenIddict 客户端模块，提供了使用 Dapr 服务调用构建块来调用 OpenIddict 远程服务的功能。

[English](./README.EN.md)

## 功能

* Dapr 服务调用集成
  * 自动注册 OpenIddict 应用程序契约的 Dapr 客户端代理
  * 支持通过 Dapr 服务调用访问 OpenIddict 远程服务
  * 支持分布式系统中的服务间通信

* 远程服务支持
  * 支持所有 OpenIddict 应用程序契约定义的服务
  * 支持应用程序管理
  * 支持授权管理
  * 支持作用域管理
  * 支持令牌管理

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Dapr.Client
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictDaprClientModule))]` 到你的模块类。

2. 配置 Dapr 服务调用：

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpDaprClientOptions>(options =>
    {
        options.ApplicationServices.Configure(config =>
        {
            // 配置 OpenIddict 服务的 AppId
            config.AppId = "openiddict-service";
        });
    });
}
```

3. 使用示例：

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
        // 通过 Dapr 服务调用访问 OpenIddict 应用程序服务
        var applications = await _applicationAppService.GetListAsync(
            new GetApplicationsInput());
    }
}
```

## 配置

* AppId
  * OpenIddict 服务的应用程序标识符
  * 必须与 Dapr 组件配置中的应用 ID 匹配

* RemoteServiceName
  * OpenIddict 远程服务的名称
  * 默认值为 "OpenIddict"

## 注意事项

* 确保 Dapr Sidecar 已正确配置和运行
* 确保 OpenIddict 服务已在 Dapr 中注册
* 建议在生产环境中配置服务间的身份认证
* 建议配置服务调用的重试策略
* 建议配置服务发现机制
