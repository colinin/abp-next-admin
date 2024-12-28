# LINGYUN.Abp.OpenIddict.WeChat.Work

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2Fcolinin%2Fabp-next-admin%2Fmaster%2Faspnet-core%2Fmodules%2FopenIddict%2FLINGYUN.Abp.OpenIddict.WeChat.Work%2FLINGYUN.Abp.OpenIddict.WeChat.Work.csproj)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/LINGYUN.Abp.OpenIddict.WeChat.Work.svg?style=flat-square)](https://www.nuget.org/packages/LINGYUN.Abp.OpenIddict.WeChat.Work)

## 介绍

`LINGYUN.Abp.OpenIddict.WeChat.Work` 是一个基于 OpenIddict 的企业微信认证扩展模块，支持企业微信的认证流程。

[English](./README.EN.md)

## 功能

* 企业微信认证
  * 支持企业微信授权登录
  * 支持自动注册新用户
  * 支持跨租户认证
  * 支持安全日志记录

* 用户管理集成
  * 支持企业微信账号绑定
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
dotnet add package LINGYUN.Abp.OpenIddict.WeChat.Work
```

## 使用

1. 添加 `[DependsOn(typeof(AbpOpenIddictWeChatWorkModule))]` 到你的模块类。

2. 配置 OpenIddict 服务器：

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<OpenIddictServerBuilder>(builder =>
    {
        // 允许企业微信认证流程
        builder.AllowWeChatWorkFlow();
    });
}
```

3. 使用示例：

企业微信认证：
```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=wechat_work&
agent_id=1000001&
code=021iZ1Ga1TpLOB0dXJIa1Zr6RR1iZ1G7&
scope=openid profile wechat_work
```

## 参数

* agent_id (必填)
  * 企业微信应用ID
  * 必须是有效的企业微信应用ID

* code (必填)
  * 企业微信授权码
  * 必须是有效的企业微信授权码

* scope (可选)
  * 请求的权限范围
  * 默认包含 openid profile wechat_work

## 错误码

* invalid_grant
  * GrantTypeInvalid - 不被允许的授权类型
  * WeChatTokenInvalid - 企业微信认证失败
  * AgentIdOrCodeNotFound - 应用ID或授权码为空或不存在
  * UserIdNotRegister - 用户企业微信账号未绑定

## 注意事项

* 必须配置正确的企业微信应用ID和密钥
* 必须配置正确的企业微信企业ID
* 授权码有效期有限
* 多次认证失败可能导致账号锁定
* 所有认证操作都会记录在安全日志中
* 生产环境建议使用 HTTPS
