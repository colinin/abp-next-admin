# LINGYUN.Abp.OpenIddict.Portal

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Portal%2FLINGYUN.Abp.OpenIddict.Portal.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Portal.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Portal)

## 简介

`LINGYUN.Abp.OpenIddict.Portal` 是 OpenIddict 的门户认证扩展模块，提供了企业门户的认证功能，包括多租户选择、双因素认证、密码策略等特性。

[English](./README.EN.md)

## 功能特性

* 企业门户认证
  * 支持企业列表选择
  * 支持多租户认证
  * 支持企业 Logo 显示

* 密码认证增强
  * 支持邮箱登录
  * 支持外部登录提供程序
  * 支持密码策略验证
  * 支持强制修改密码
  * 支持定期修改密码

* 双因素认证
  * 支持多种验证提供程序
  * 支持验证码验证
  * 支持认证器验证

* 安全日志
  * 记录登录尝试
  * 记录登录失败
  * 记录密码修改

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Portal
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictPortalModule))]` 到你的模块类。

2. 配置 OpenIddict 服务器:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 允许门户认证流程
        builder.AllowPortalFlow();
    });
}
```

3. 使用示例:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=portal&
username=admin&
password=1q2w3E*&
enterpriseId=your-enterprise-id&
scope=openid profile
```

## 认证流程

1. 企业选择
   * 用户提供企业ID (enterpriseId)
   * 如未提供或无效，返回可选企业列表

2. 密码验证
   * 支持用户名或邮箱登录
   * 验证密码策略
   * 检查账户状态

3. 双因素认证 (如启用)
   * 验证双因素认证码
   * 支持多种验证提供程序

4. 密码修改 (如需要)
   * 首次登录强制修改密码
   * 定期修改密码要求

## 参数说明

* username (必填)
  * 用户名或邮箱地址

* password (必填)
  * 用户密码

* enterpriseId (必填)
  * 企业ID，必须是有效的GUID格式

* TwoFactorProvider (可选)
  * 双因素认证提供程序名称
  * 仅在启用双因素认证时需要

* TwoFactorCode (可选)
  * 双因素认证码
  * 仅在启用双因素认证时需要

* ChangePasswordToken (可选)
  * 修改密码令牌
  * 仅在需要修改密码时需要

* NewPassword (可选)
  * 新密码
  * 仅在需要修改密码时需要

## 注意事项

* 企业ID必须是有效的GUID格式
* 密码必须符合系统配置的密码策略
* 双因素认证码有效期有限
* 所有认证操作都会记录安全日志
* 建议在生产环境中使用 HTTPS
