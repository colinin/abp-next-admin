# LINGYUN.Abp.OpenIddict.Sms

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.Sms%2FLINGYUN.Abp.OpenIddict.Sms.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.Sms.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.Sms)

## 简介

`LINGYUN.Abp.OpenIddict.Sms` 是 OpenIddict 的短信验证扩展模块，提供了基于手机号码和短信验证码的认证功能。

[English](./README.EN.md)

## 功能特性

* 短信验证码认证
  * 支持手机号码登录
  * 支持短信验证码验证
  * 支持多租户认证

* 用户管理集成
  * 支持手机号码查找用户
  * 支持账户锁定策略
  * 支持失败尝试计数

* 安全日志
  * 记录登录尝试
  * 记录验证码验证失败
  * 记录账户锁定

* 多语言支持
  * 集成 ABP 本地化框架
  * 支持自定义错误消息

## 安装

```bash
dotnet add package LINGYUN.Abp.OpenIddict.Sms
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictSmsModule))]` 到你的模块类。

2. 配置 OpenIddict 服务器:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 允许短信验证码认证流程
        builder.AllowSmsFlow();
    });
}
```

3. 使用示例:

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=sms&
phone_number=13800138000&
phone_verify=123456&
scope=openid profile
```

## 参数说明

* phone_number (必填)
  * 手机号码
  * 必须是有效的手机号码格式

* phone_verify (必填)
  * 短信验证码
  * 必须是有效的验证码

* scope (可选)
  * 请求的权限范围
  * 默认包含 openid profile

## 错误码说明

* invalid_grant
  * GrantTypeInvalid - 不被允许的授权类型
  * PhoneVerifyInvalid - 手机验证码无效或已过期
  * PhoneOrTokenCodeNotFound - 手机号码或验证码未输入
  * PhoneNumberNotRegister - 登录的手机号码未注册

## 注意事项

* 手机号码必须已经注册
* 验证码有效期有限
* 验证失败会增加失败计数
* 多次验证失败可能导致账户锁定
* 所有认证操作都会记录安全日志
* 建议在生产环境中使用 HTTPS
