# LINGYUN.Abp.OpenIddict.AspNetCore

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.AspNetCore%2FLINGYUN.Abp.OpenIddict.AspNetCore.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.AspNetCore.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.AspNetCore)

## Introduction

`LINGYUN.Abp.OpenIddict.AspNetCore` is an ASP.NET Core integration module based on ABP OpenIddict, providing user information extensions and avatar URL claim support.

[简体中文](./README.md)

## Features

* Extended UserInfo Endpoint
  * Support for returning user avatar URL
  * Extended standard OpenID Connect claims
  * Multi-tenancy support

* Avatar URL Claims Contributor
  * Automatically add user avatar URL claim
  * Integration with identity system

* OpenIddict Server Configuration
  * Pre-configured avatar URL claim support
  * Inherits from Volo.Abp.OpenIddict.AbpOpenIddictAspNetCoreModule

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.AspNetCore
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictAspNetCoreModule))]` to your module class.

2. The UserInfo endpoint will automatically be extended to include the following claims:
   * sub (User ID)
   * tenant_id (Tenant ID, if multi-tenancy is enabled)
   * preferred_username (Username)
   * family_name (Surname)
   * given_name (Name)
   * picture (Avatar URL)
   * email
   * email_verified
   * phone_number
   * phone_number_verified
   * role

3. The avatar URL claim will be automatically added to the user's identity claims.

## Configuration

By default, the module is pre-configured with the required settings. If you need to customize the configuration, you can modify it in the module's `PreConfigureServices` method:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // Custom configuration
        builder.RegisterClaims(new[] { "your_custom_claim" });
    });
}
```

## Notes

* The avatar URL claim type is `IdentityConsts.ClaimType.Avatar.Name`
* The UserInfo endpoint requires corresponding scope permissions to return information:
  * profile: Basic information and avatar
  * email: Email-related information
  * phone: Phone number-related information
  * roles: User role information
