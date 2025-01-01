# LINGYUN.Abp.OpenIddict.Sms

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Sms%2FLINGYUN.Abp.OpenIddict.Sms.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Sms.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Sms)

## Introduction

`LINGYUN.Abp.OpenIddict.Sms` is an SMS verification extension module for OpenIddict, providing authentication functionality based on phone numbers and SMS verification codes.

[简体中文](./README.md)

## Features

* SMS Verification Code Authentication
  * Support for phone number login
  * Support for SMS verification code validation
  * Support for multi-tenant authentication

* User Management Integration
  * Support for finding users by phone number
  * Support for account lockout policy
  * Support for failed attempt counting

* Security Logging
  * Record login attempts
  * Record verification code failures
  * Record account lockouts

* Localization Support
  * Integrated with ABP localization framework
  * Support for custom error messages

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Sms
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictSmsModule))]` to your module class.

2. Configure OpenIddict server:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // Allow SMS verification code authentication flow
        builder.AllowSmsFlow();
    });
}
```

3. Usage example:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=sms&
phone_number=13800138000&
phone_verify=123456&
scope=openid profile
```

## Parameters

* phone_number (required)
  * Phone number
  * Must be a valid phone number format

* phone_verify (required)
  * SMS verification code
  * Must be a valid verification code

* scope (optional)
  * Requested permission scopes
  * Defaults include openid profile

## Error Codes

* invalid_grant
  * GrantTypeInvalid - Authorization type not allowed
  * PhoneVerifyInvalid - Phone verification code invalid or expired
  * PhoneOrTokenCodeNotFound - Phone number or verification code not provided
  * PhoneNumberNotRegister - Phone number not registered

## Notes

* Phone number must be registered
* Verification codes have limited validity
* Failed verifications increase failure count
* Multiple verification failures may lead to account lockout
* All authentication operations are recorded in security logs
* HTTPS is recommended in production environments
