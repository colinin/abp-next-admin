# LINGYUN.Abp.Authentication 认证模块

本模块提供第三方社交登录认证功能，目前支持 QQ 和微信公众号登录，并与 ABP 身份系统深度集成。

## 模块概述

认证模块包含两个主要子模块：

1. **QQ 互联认证模块**

   - 支持 QQ OAuth2.0 认证
   - 适用于移动端和 PC 端登录
   - 获取用户基本信息（昵称、性别、头像等）

2. **微信公众号认证模块**
   - 支持微信公众号 OAuth2.0 认证
   - 获取用户详细信息（昵称、性别、地区、头像等）
   - 支持 UnionId 机制，打通公众号与小程序账号体系

## 功能特性

- 第三方社交账号登录
- 获取用户基本信息
- 与 ABP 身份系统无缝集成
- 灵活的配置选项
- 支持多种登录场景（移动端、PC 端）

## 快速开始

### 模块引用

```csharp
[DependsOn(
    typeof(AbpAuthenticationQQModule),
    typeof(AbpAuthenticationWeChatModule)
)]
public class YourProjectModule : AbpModule
{
    // 其他配置
}
```

### 配置示例

在 `appsettings.json` 中配置第三方登录参数：

```json
{
  "Authentication": {
    "QQ": {
      "AppId": "你的QQ互联AppId",
      "AppKey": "你的QQ互联AppKey"
    },
    "WeChat": {
      "AppId": "你的微信公众号AppId",
      "AppSecret": "你的微信公众号AppSecret"
    }
  }
}
```

### 添加登录支持

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAuthentication()
        .AddQQConnect()   // 添加QQ登录
        .AddWeChat();     // 添加微信登录
}
```

## 支持的用户信息

### QQ 登录获取信息

- OpenId - QQ 用户唯一标识
- NickName - 用户昵称
- Gender - 用户性别
- AvatarUrl - 用户头像 URL

### 微信登录获取信息

- OpenId - 微信用户唯一标识
- UnionId - 微信开放平台唯一标识
- NickName - 用户昵称
- Sex - 用户性别
- Country, Province, City - 地理信息
- AvatarUrl - 用户头像 URL

## 参考文档

- [QQ 互联文档](https://wiki.connect.qq.com/)
- [微信公众平台开发文档](https://developers.weixin.qq.com/doc/offiaccount/Getting_Started/Overview.html)
- [ABP 认证文档](https://docs.abp.io/en/abp/latest/Authentication)

## 许可证

遵循项目的开源许可证
