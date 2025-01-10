# LINGYUN.Abp.Account.HttpApi

ABP账户模块的HTTP API层，提供RESTful API接口实现。

[English](./README.EN.md)

## 功能特性

* 提供账户管理的HTTP API接口
* 支持本地化和多语言
* 集成ABP MVC框架
* 自动API控制器注册

## 模块引用

```csharp
[DependsOn(
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API控制器

### AccountController

提供以下HTTP API端点：
* POST /api/account/register - 手机号注册
* POST /api/account/register-by-wechat - 微信小程序注册
* POST /api/account/reset-password - 重置密码
* POST /api/account/send-phone-register-code - 发送手机注册验证码
* POST /api/account/send-phone-signin-code - 发送手机登录验证码
* POST /api/account/send-email-signin-code - 发送邮箱登录验证码
* POST /api/account/send-phone-reset-password-code - 发送手机重置密码验证码

### MyProfileController

提供以下HTTP API端点：
* GET /api/account/my-profile - 获取个人资料
* PUT /api/account/my-profile - 更新个人资料
* POST /api/account/my-profile/change-password - 修改密码
* POST /api/account/my-profile/change-phone-number - 修改手机号
* POST /api/account/my-profile/send-phone-number-change-code - 发送手机号变更验证码
* POST /api/account/my-profile/change-avatar - 更新头像

### MyClaimController

提供以下HTTP API端点：
* GET /api/account/my-claim - 获取用户声明
* PUT /api/account/my-claim - 更新用户声明

## 本地化配置

模块预配置了本地化选项：
* 支持AccountResource资源本地化
* 自动注册MVC应用程序部件
