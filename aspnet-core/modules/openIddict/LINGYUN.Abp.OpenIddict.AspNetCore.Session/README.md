# LINGYUN.Abp.OpenIddict.AspNetCore.Session

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.AspNetCore.Session%2FLINGYUN.Abp.OpenIddict.AspNetCore.Session.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.AspNetCore.Session.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.AspNetCore.Session)

## 简介

`LINGYUN.Abp.OpenIddict.AspNetCore.Session` 是 OpenIddict 的会话管理扩展模块，提供了用户会话的持久化、验证和撤销功能。

[English](./README.EN.md)

## 功能特性

* 用户会话管理
  * 登录时自动持久化会话
  * 登出时自动终止会话
  * 令牌撤销时自动终止会话
  * UserInfo 端点会话验证

* 多租户支持
  * 会话管理支持多租户场景
  * 租户隔离的会话存储

* 可配置的会话持久化
  * 支持配置需要持久化会话的授权类型
  * 默认支持密码授权类型

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.AspNetCore.Session
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictAspNetCoreSessionModule))]` 到你的模块类。

2. 配置会话选项：

```csharp
Configure<IdentitySessionSignInOptions>(options =>
{
    options.SignInSessionEnabled = true;    // 启用登录会话
    options.SignOutSessionEnabled = true;   // 启用登出会话
});

Configure<AbpOpenIddictAspNetCoreSessionOptions>(options =>
{
    // 配置需要持久化会话的授权类型
    options.PersistentSessionGrantTypes.Add(GrantTypes.Password);
    options.PersistentSessionGrantTypes.Add("custom_grant_type");
});
```

## 工作流程

1. 用户登录
   * 当用户通过配置的授权类型登录成功时
   * 系统自动创建并持久化会话信息

2. 会话验证
   * 当用户访问 UserInfo 端点时
   * 系统自动验证会话是否有效
   * 如果会话已过期或无效，返回相应的错误

3. 会话终止
   * 用户主动登出时自动终止会话
   * 令牌撤销时自动终止相关会话
   * 支持多终端同时登录的会话管理

## 注意事项

* 会话持久化仅对配置的授权类型生效
* 会话验证在多租户环境中会自动切换租户上下文
* 令牌撤销会同时终止相关的用户会话
* UserInfo 端点的会话验证是强制的，无效会话将导致请求被拒绝
