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

grant_type=qr_code&
qrcode_key=a73faf8936f649868223a9287317d8d1&
scope=openid profile
```

## 认证流程

1. 二维码验证
   * 用户提供二维码Key (qrcode_key_)
   * 如未提供或无效，返回无效的二维码错误

## 参数说明

* qrcode_key_ (必填)
  * 二维码Key

## 注意事项

* 所有认证操作都会记录安全日志
* 建议在生产环境中使用 HTTPS
