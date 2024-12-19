# LINGYUN.Abp.OpenIddict.AspNetCore

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.AspNetCore%2FLINGYUN.Abp.OpenIddict.AspNetCore.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.AspNetCore.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.AspNetCore)

## 简介

`LINGYUN.Abp.OpenIddict.AspNetCore` 是基于 ABP OpenIddict 的 ASP.NET Core 集成模块，提供了用户信息扩展和头像URL声明支持。

[English](./README.EN.md)

## 功能特性

* 扩展用户信息端点
  * 支持返回用户头像URL
  * 扩展标准 OpenID Connect 声明
  * 支持多租户

* 头像URL声明贡献者
  * 自动添加用户头像URL声明
  * 与身份系统集成

* OpenIddict 服务器配置
  * 预配置头像URL声明支持
  * 继承自 Volo.Abp.OpenIddict.AbpOpenIddictAspNetCoreModule

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.AspNetCore
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictAspNetCoreModule))]` 到你的模块类。

2. 用户信息端点将自动扩展以包含以下声明：
   * sub (用户ID)
   * tenant_id (租户ID，如果启用了多租户)
   * preferred_username (用户名)
   * family_name (姓氏)
   * given_name (名字)
   * picture (头像URL)
   * email
   * email_verified
   * phone_number
   * phone_number_verified
   * role

3. 头像URL声明会自动添加到用户的身份声明中。

## 配置

默认情况下，模块已预配置了所需的设置。如果需要自定义配置，可以在模块的 `PreConfigureServices` 方法中修改：

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 自定义配置
        builder.RegisterClaims(new[] { "your_custom_claim" });
    });
}
```

## 注意事项

* 头像URL声明的类型为 `IdentityConsts.ClaimType.Avatar.Name`
* 用户信息端点需要相应的作用域权限才能返回信息：
  * profile: 基本信息和头像
  * email: 电子邮件相关信息
  * phone: 电话号码相关信息
  * roles: 用户角色信息
