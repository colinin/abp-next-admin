# LINGYUN.Abp.OpenIddict.WeChat.Work

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.WeChat.Work%2FLINGYUN.Abp.OpenIddict.WeChat.Work.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.WeChat.Work.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.WeChat.Work)

## Introduction

`LINGYUN.Abp.OpenIddict.WeChat.Work` is a WeChat Work (Enterprise WeChat) authentication extension module based on OpenIddict, supporting WeChat Work authentication flow.

[简体中文](./README.md)

## Features

* WeChat Work Authentication
  * Support WeChat Work authorization login
  * Support automatic new user registration
  * Support cross-tenant authentication
  * Support security logging

* User Management Integration
  * Support WeChat Work account binding
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
dotnet add package LINGYUN.Abp.OpenIddict.WeChat.Work
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictWeChatWorkModule))]` to your module class.

2. Configure OpenIddict server:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // Allow WeChat Work authentication flow
        builder.AllowWeChatWorkFlow();
    });
}
```

3. Usage example:

WeChat Work Authentication:
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=wechat_work&
agent_id=1000001&
code=021iZ1Ga1TpLOB0dXJIa1Zr6RR1iZ1G7&
scope=openid profile wechat_work
```

## Parameters

* agent_id (required)
  * WeChat Work application ID
  * Must be a valid WeChat Work application ID

* code (required)
  * WeChat Work authorization code
  * Must be a valid WeChat Work authorization code

* scope (optional)
  * Requested permission scopes
  * Defaults include openid profile wechat_work

## Error Codes

* invalid_grant
  * GrantTypeInvalid - Authorization type not allowed
  * WeChatTokenInvalid - WeChat Work authentication failed
  * AgentIdOrCodeNotFound - Application ID or authorization code empty or not found
  * UserIdNotRegister - User WeChat Work account not bound

## Notes

* Must configure correct WeChat Work application ID and secret
* Must configure correct WeChat Work enterprise ID
* Authorization codes have limited validity
* Multiple authentication failures may lead to account lockout
* All authentication operations are recorded in security logs
* HTTPS is recommended in production environments
