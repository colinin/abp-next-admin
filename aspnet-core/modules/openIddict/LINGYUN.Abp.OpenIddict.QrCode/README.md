# LINGYUN.Abp.OpenIddict.QrCode

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.QrCode%2FLINGYUN.Abp.OpenIddict.QrCode.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.QrCode.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.QrCode)

## 简介

`LINGYUN.Abp.OpenIddict.QrCode` 是 OpenIddict 的扫码登录认证扩展模块，提供了扫码登录认证功能。

[English](./README.EN.md)

## 功能特性

* 扫码二维码登录

* 安全日志
  * 记录登录尝试
  * 记录登录失败
  * 记录密码修改

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.QrCode
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictQrCodeModule))]` 到你的模块类。

2. 配置 OpenIddict 服务器:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 允许门户认证流程
        builder.AllowQrCodeFlow();
    });
}
```

3. 使用示例:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=qr_code_&
username=admin&
password=1q2w3E*&
enterpriseId=your-enterprise-id&
scope=openid profile
```

## 认证流程

1. 二维码验证
   * 用户提供二维码Key (qrcode_key_)
   * 如未提供或无效，返回无效的二维码错误

## 参数说明

* qrcode_key_ (必填)
  * 二维码Key

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
