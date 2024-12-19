# LINGYUN.Abp.OpenIddict.HttpApi.Client

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.HttpApi.Client%2FLINGYUN.Abp.OpenIddict.HttpApi.Client.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.HttpApi.Client.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.HttpApi.Client)

## 简介

`LINGYUN.Abp.OpenIddict.HttpApi.Client` 是 OpenIddict 的 HTTP API 客户端模块，提供了远程调用 OpenIddict HTTP API 的客户端代理。

[English](./README.EN.md)

## 功能特性

* HTTP API 客户端代理
  * 自动生成 HTTP 客户端代理
  * 支持远程服务调用
  * 集成 ABP 动态 HTTP 客户端代理

* 远程服务配置
  * 支持配置远程服务地址
  * 支持配置认证方式
  * 支持配置请求头

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.HttpApi.Client
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictHttpApiClientModule))]` 到你的模块类。

2. 配置远程服务:

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

3. 使用示例:

```csharp
// 注入客户端代理
private readonly IOpenIddictApplicationAppService _applicationService;

public YourService(IOpenIddictApplicationAppService applicationService)
{
    _applicationService = applicationService;
}

// 调用远程服务
var input = new OpenIddictApplicationCreateDto
{
    ClientId = "your-client-id",
    DisplayName = "Your App",
    // ... 其他属性
};
var result = await _applicationService.CreateAsync(input);
```

## 配置项

* RemoteServices
  * Default:BaseUrl - 默认远程服务地址
  * OpenIddict:BaseUrl - OpenIddict 远程服务地址

## 注意事项

* 需要配置正确的远程服务地址
* 如果远程服务需要认证，需要配置相应的认证信息
* 建议在生产环境中使用 HTTPS
* 客户端代理会自动处理认证令牌的传递
