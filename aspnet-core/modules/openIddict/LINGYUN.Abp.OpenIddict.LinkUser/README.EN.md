# LINGYUN.Abp.OpenIddict.LinkUser

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.LinkUser%2FLINGYUN.Abp.OpenIddict.LinkUser.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.LinkUser.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.LinkUser)

## Introduction

`LINGYUN.Abp.OpenIddict.LinkUser` is a user linking extension module for OpenIddict, providing authentication functionality between linked users.

[简体中文](./README.md)

## Features

* User Link Authentication
  * Support for user-to-user linking authentication
  * Support for cross-tenant user linking
  * Support for access token exchange

* Extension Grant Type
  * Add link_user grant type
  * Support for custom authorization flow
  * Security log recording

* Localization Support
  * Integrated with ABP localization framework
  * Support for custom error messages

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.LinkUser
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictLinkUserModule))]` to your module class.

2. Configure OpenIddict server:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // Allow link_user grant type
        builder.AllowLinkUserFlow();
    });
}
```

3. Usage example:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=link_user&
access_token=current-user-access-token&
LinkUserId=target-user-id&
LinkTenantId=target-tenant-id&
scope=openid profile
```

## Parameters

* access_token (required)
  * Current user's access token
  * Used to verify current user's identity

* LinkUserId (required)
  * Target linked user's ID
  * Must be a valid GUID format

* LinkTenantId (optional)
  * Tenant ID of the target linked user
  * If specified, must be a valid GUID format

* scope (optional)
  * Requested permission scopes
  * Defaults include openid profile

## Notes

* A valid access token must be provided
* The target user must have a linking relationship with the current user
* Correct tenant ID must be specified for cross-tenant linking
* All operations are recorded in security logs
* HTTPS is recommended in production environments
