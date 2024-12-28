# LINGYUN.Abp.OpenIddict.WeChat

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.WeChat%2FLINGYUN.Abp.OpenIddict.WeChat.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.WeChat.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.WeChat)

## 介绍

`LINGYUN.Abp.OpenIddict.WeChat` 是一个基于 OpenIddict 的微信认证扩展模块，支持微信公众平台和微信小程序的认证流程。

[English](./README.EN.md)

## 功能

* 微信公众平台认证
  * 支持公众号授权登录
  * 支持自动注册新用户
  * 支持跨租户认证
  * 支持安全日志记录

* 微信小程序认证
  * 支持小程序授权登录
  * 支持自动注册新用户
  * 支持跨租户认证
  * 支持安全日志记录

* 用户管理集成
  * 支持微信账号绑定
  * 支持账号锁定策略
  * 支持失败尝试计数

* 安全日志
  * 记录登录尝试
  * 记录认证失败
  * 记录账号锁定

* 本地化支持
  * 集成 ABP 本地化框架
  * 支持自定义错误消息

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.WeChat
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictWeChatModule))]` 到你的模块类。

2. 配置 OpenIddict 服务器：

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 允许微信认证流程
        builder.AllowWeChatFlow();
    });
}
```

3. 使用示例：

微信公众平台认证：
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=wechat_official&
code=021iZ1Ga1TpLOB0dXJIa1Zr6RR1iZ1G7&
scope=openid profile wechat
```

微信小程序认证：
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=wechat_mini_program&
code=021iZ1Ga1TpLOB0dXJIa1Zr6RR1iZ1G7&
scope=openid profile wechat
```

## 参数

* code (必填)
  * 微信授权码
  * 必须是有效的微信授权码

* scope (可选)
  * 请求的权限范围
  * 默认包含 openid profile wechat

## 错误码

* invalid_grant
  * GrantTypeInvalid - 不被允许的授权类型
  * WeChatTokenInvalid - 微信认证失败
  * WeChatCodeNotFound - 微信授权码为空或不存在
  * WeChatNotRegister - 用户微信账号未绑定

## 注意事项

* 必须启用相应的功能（公众平台或小程序授权）
* 必须配置正确的微信应用 ID 和密钥
* 授权码有效期有限
* 多次认证失败可能导致账号锁定
* 所有认证操作都会记录在安全日志中
* 生产环境建议使用 HTTPS
