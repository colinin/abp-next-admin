# LINGYUN.Abp.OpenIddict.Application

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Application%2FLINGYUN.Abp.OpenIddict.Application.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Application.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Application)

## Introduction

`LINGYUN.Abp.OpenIddict.Application` is an application service layer implementation based on ABP OpenIddict, providing management functionality for OpenIddict clients, authorizations, tokens, and scopes.

[简体中文](./README.md)

## Features

* OpenIddict Client Application Management
  * Create, update, and delete client applications
  * Manage client basic information (client ID, secret, name, etc.)
  * Configure grant types, response types, scopes, and redirect URIs
  * Support custom configuration of client properties and requirements

* OpenIddict Authorization Management
  * Query and delete authorization records
  * Filter authorizations by client ID, creation time, status, etc.

* OpenIddict Token Management
  * Query and delete tokens
  * Filter tokens by client ID, creation time, expiration time, status, etc.

* OpenIddict Scope Management
  * Create, update, and delete scopes
  * Manage scope basic information (name, description, display name, etc.)
  * Support multilingual display names and descriptions
  * Configure resources associated with scopes

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Application
```

## Usage

1. Add `[DependsOn(typeof(AbpOpenIddictApplicationModule))]` to your module class.

2. Configure permissions:

The module predefines the following permissions:

* OpenIddict.Applications: Client application management
  * OpenIddict.Applications.Create: Create clients
  * OpenIddict.Applications.Update: Update clients
  * OpenIddict.Applications.Delete: Delete clients
* OpenIddict.Authorizations: Authorization management
  * OpenIddict.Authorizations.Delete: Delete authorizations
* OpenIddict.Scopes: Scope management
  * OpenIddict.Scopes.Create: Create scopes
  * OpenIddict.Scopes.Update: Update scopes
  * OpenIddict.Scopes.Delete: Delete scopes
* OpenIddict.Tokens: Token management
  * OpenIddict.Tokens.Delete: Delete tokens

## Notes

* Client application ClientId cannot be modified after creation
* Deleting a client application will also delete related authorizations and tokens
* Scope names cannot be modified after creation
