# LINGYUN.Abp.OpenIddict.WeChat

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.WeChat%2FLINGYUN.Abp.OpenIddict.WeChat.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.WeChat.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.WeChat)

## Introduction

`LINGYUN.Abp.OpenIddict.WeChat` is a WeChat authentication extension module based on OpenIddict, supporting authentication flows for WeChat Official Platform and WeChat Mini Program.

[简体中文](./README.md)

## Features

* WeChat Official Platform Authentication
  * Support Official Account authorization login
  * Support automatic new user registration
  * Support cross-tenant authentication
  * Support security logging

* WeChat Mini Program Authentication
  * Support Mini Program authorization login
  * Support automatic new user registration
  * Support cross-tenant authentication
  * Support security logging

* User Management Integration
  * Support WeChat account binding
  * Support account lockout policy
  * Support failed attempt counting

* Security Logging
  * Record login attempts
  * Record authentication failures
  * Record account lockouts

* Localization Support
  * Integrated with ABP localization framework
  * Support custom error messages

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.WeChat
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictWeChatModule))]` to your module class.

2. Configure OpenIddict server:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // Allow WeChat authentication flow
        builder.AllowWeChatFlow();
    });
}
```

3. Usage examples:

WeChat Official Platform Authentication:
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=wechat_official&
code=021iZ1Ga1TpLOB0dXJIa1Zr6RR1iZ1G7&
scope=openid profile wechat
```

WeChat Mini Program Authentication:
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=wechat_mini_program&
code=021iZ1Ga1TpLOB0dXJIa1Zr6RR1iZ1G7&
scope=openid profile wechat
```

## Parameters

* code (required)
  * WeChat authorization code
  * Must be a valid WeChat authorization code

* scope (optional)
  * Requested permission scopes
  * Defaults include openid profile wechat

## Error Codes

* invalid_grant
  * GrantTypeInvalid - Authorization type not allowed
  * WeChatTokenInvalid - WeChat authentication failed
  * WeChatCodeNotFound - WeChat authorization code empty or not found
  * WeChatNotRegister - User WeChat account not bound

## Notes

* Must enable corresponding features (Official Platform or Mini Program authorization)
* Must configure correct WeChat application ID and secret
* Authorization codes have limited validity
* Multiple authentication failures may lead to account lockout
* All authentication operations are recorded in security logs
* HTTPS is recommended in production environments
