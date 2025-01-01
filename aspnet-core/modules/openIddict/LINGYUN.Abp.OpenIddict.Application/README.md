# LINGYUN.Abp.OpenIddict.Application

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Application%2FLINGYUN.Abp.OpenIddict.Application.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Application.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Application)

## 简介

`LINGYUN.Abp.OpenIddict.Application` 是基于 ABP OpenIddict 的应用服务层实现，提供了对 OpenIddict 客户端、授权、令牌和作用域的管理功能。

[English](./README.EN.md)

## 功能特性

* OpenIddict 客户端应用程序管理
  * 创建、更新、删除客户端应用程序
  * 管理客户端基本信息（客户端ID、密钥、名称等）
  * 配置授权类型、响应类型、作用域和重定向URI
  * 支持客户端属性和要求的自定义配置

* OpenIddict 授权管理
  * 查询和删除授权记录
  * 按客户端ID、创建时间、状态等条件筛选授权

* OpenIddict 令牌管理
  * 查询和删除令牌
  * 按客户端ID、创建时间、过期时间、状态等条件筛选令牌

* OpenIddict 作用域管理
  * 创建、更新、删除作用域
  * 管理作用域基本信息（名称、描述、显示名称等）
  * 支持多语言显示名称和描述
  * 配置作用域关联的资源

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Application
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictApplicationModule))]` 到你的模块类。

2. 配置权限:

模块预定义了以下权限:

* OpenIddict.Applications: 客户端应用管理
  * OpenIddict.Applications.Create: 创建客户端
  * OpenIddict.Applications.Update: 更新客户端
  * OpenIddict.Applications.Delete: 删除客户端
* OpenIddict.Authorizations: 授权管理
  * OpenIddict.Authorizations.Delete: 删除授权
* OpenIddict.Scopes: 作用域管理
  * OpenIddict.Scopes.Create: 创建作用域
  * OpenIddict.Scopes.Update: 更新作用域
  * OpenIddict.Scopes.Delete: 删除作用域
* OpenIddict.Tokens: 令牌管理
  * OpenIddict.Tokens.Delete: 删除令牌

## 注意事项

* 客户端应用程序的 ClientId 在创建后不能修改
* 删除客户端应用程序会同时删除相关的授权和令牌
* 作用域名称在创建后不能修改
