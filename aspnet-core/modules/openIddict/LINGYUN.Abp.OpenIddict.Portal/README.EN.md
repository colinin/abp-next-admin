# LINGYUN.Abp.OpenIddict.Portal

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Portal%2FLINGYUN.Abp.OpenIddict.Portal.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Portal.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Portal)

## Introduction

`LINGYUN.Abp.OpenIddict.Portal` is a portal authentication extension module for OpenIddict, providing enterprise portal authentication functionality, including multi-tenant selection, two-factor authentication, password policies, and more.

[简体中文](./README.md)

## Features

* Enterprise Portal Authentication
  * Support for enterprise list selection
  * Support for multi-tenant authentication
  * Support for enterprise logo display

* Enhanced Password Authentication
  * Support for email login
  * Support for external login providers
  * Support for password policy validation
  * Support for forced password change
  * Support for periodic password change

* Two-Factor Authentication
  * Support for multiple verification providers
  * Support for verification code validation
  * Support for authenticator validation

* Security Logging
  * Record login attempts
  * Record login failures
  * Record password changes

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Portal
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictPortalModule))]` to your module class.

2. Configure OpenIddict server:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // Allow portal authentication flow
        builder.AllowPortalFlow();
    });
}
```

3. Usage example:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=portal&
username=admin&
password=1q2w3E*&
enterpriseId=your-enterprise-id&
scope=openid profile
```

## Authentication Flow

1. Enterprise Selection
   * User provides enterprise ID (enterpriseId)
   * Returns available enterprise list if not provided or invalid

2. Password Verification
   * Support for username or email login
   * Password policy validation
   * Account status check

3. Two-Factor Authentication (if enabled)
   * Verify two-factor authentication code
   * Support for multiple verification providers

4. Password Change (if required)
   * Force password change on first login
   * Periodic password change requirement

## Parameters

* username (required)
  * Username or email address

* password (required)
  * User password

* enterpriseId (required)
  * Enterprise ID, must be a valid GUID format

* TwoFactorProvider (optional)
  * Two-factor authentication provider name
  * Only required when two-factor authentication is enabled

* TwoFactorCode (optional)
  * Two-factor authentication code
  * Only required when two-factor authentication is enabled

* ChangePasswordToken (optional)
  * Password change token
  * Only required when password change is needed

* NewPassword (optional)
  * New password
  * Only required when password change is needed

## Notes

* Enterprise ID must be a valid GUID format
* Password must comply with system-configured password policy
* Two-factor authentication codes have limited validity
* All authentication operations are recorded in security logs
* HTTPS is recommended in production environments
