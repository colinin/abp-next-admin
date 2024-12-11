# LINGYUN.Abp.OpenIddict.LinkUser

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.LinkUser%2FLINGYUN.Abp.OpenIddict.LinkUser.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.LinkUser.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.LinkUser)

## 简介

`LINGYUN.Abp.OpenIddict.LinkUser` 是 OpenIddict 的用户链接扩展模块，提供了用户之间的链接认证功能。

[English](./README.EN.md)

## 功能特性

* 用户链接认证
  * 支持用户之间的链接认证
  * 支持跨租户用户链接
  * 支持访问令牌交换

* 扩展授权类型
  * 添加 link_user 授权类型
  * 支持自定义授权流程
  * 安全日志记录

* 多语言支持
  * 集成 ABP 本地化框架
  * 支持自定义错误消息

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.LinkUser
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictLinkUserModule))]` 到你的模块类。

2. 配置 OpenIddict 服务器:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 允许 link_user 授权类型
        builder.AllowLinkUserFlow();
    });
}
```

3. 使用示例:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=link_user&
access_token=current-user-access-token&
LinkUserId=target-user-id&
LinkTenantId=target-tenant-id&
scope=openid profile
```

## 参数说明

* access_token (必填)
  * 当前用户的访问令牌
  * 用于验证当前用户身份

* LinkUserId (必填)
  * 目标链接用户的ID
  * 必须是有效的GUID格式

* LinkTenantId (可选)
  * 目标链接用户所属的租户ID
  * 如果指定，必须是有效的GUID格式

* scope (可选)
  * 请求的权限范围
  * 默认包含 openid profile

## 注意事项

* 必须提供有效的访问令牌
* 目标用户必须与当前用户存在链接关系
* 跨租户链接时需要指定正确的租户ID
* 所有操作都会记录安全日志
* 建议在生产环境中使用 HTTPS
