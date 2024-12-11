# LINGYUN.Abp.OpenIddict.AspNetCore.Session

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.AspNetCore.Session%2FLINGYUN.Abp.OpenIddict.AspNetCore.Session.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.AspNetCore.Session.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.AspNetCore.Session)

## Introduction

`LINGYUN.Abp.OpenIddict.AspNetCore.Session` is a session management extension module for OpenIddict, providing functionality for user session persistence, validation, and revocation.

[简体中文](./README.md)

## Features

* User Session Management
  * Automatic session persistence on login
  * Automatic session termination on logout
  * Automatic session termination on token revocation
  * UserInfo endpoint session validation

* Multi-tenancy Support
  * Session management supports multi-tenant scenarios
  * Tenant-isolated session storage

* Configurable Session Persistence
  * Support for configuring grant types that require session persistence
  * Default support for password grant type

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.AspNetCore.Session
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictAspNetCoreSessionModule))]` to your module class.

2. Configure session options:

```csharp
Configure<IdentitySessionSignInOptions>(options =>
{
    options.SignInSessionEnabled = true;    // Enable login session
    options.SignOutSessionEnabled = true;   // Enable logout session
});

Configure<AbpOpenIddictAspNetCoreSessionOptions>(options =>
{
    // Configure grant types that require session persistence
    options.PersistentSessionGrantTypes.Add(GrantTypes.Password);
    options.PersistentSessionGrantTypes.Add("custom_grant_type");
});
```

## Workflow

1. User Login
   * When a user successfully logs in through configured grant types
   * System automatically creates and persists session information

2. Session Validation
   * When user accesses the UserInfo endpoint
   * System automatically validates if the session is valid
   * Returns appropriate error if session has expired or is invalid

3. Session Termination
   * Automatically terminates session on user logout
   * Automatically terminates related sessions on token revocation
   * Supports session management for multiple concurrent logins

## Notes

* Session persistence only applies to configured grant types
* Session validation automatically switches tenant context in multi-tenant environments
* Token revocation will terminate related user sessions
* UserInfo endpoint session validation is mandatory, invalid sessions will result in request rejection
