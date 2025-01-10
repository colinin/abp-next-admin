# LINGYUN.Abp.Account.Application

ABP账户模块的应用服务实现，提供完整的账户管理功能实现。

[English](./README.EN.md)

## 功能特性

* 账户管理服务实现
  * 手机号注册
  * 微信小程序注册
  * 密码重置
  * 验证码发送（短信、邮件）
* 个人资料管理
  * 基本信息维护
  * 密码修改
  * 手机号变更
  * 头像更新
  * 双因素认证
* 用户声明管理
* 邮件服务
  * 邮件确认
  * 邮件验证
* 短信服务
  * 验证码发送
* 微信小程序集成
* 虚拟文件系统支持

## 模块引用

```csharp
[DependsOn(
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpAccountTemplatesModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务

### AccountAppService

实现`IAccountAppService`接口，提供：
* 用户注册（手机号、微信小程序）
* 密码重置
* 验证码发送

### MyProfileAppService

实现`IMyProfileAppService`接口，提供：
* 个人资料管理
* 密码修改
* 手机号变更
* 头像更新
* 双因素认证管理

### MyClaimAppService

实现`IMyClaimAppService`接口，提供：
* 用户声明管理

## 邮件服务

提供以下邮件服务：
* `IAccountEmailConfirmSender` - 邮件确认服务
* `IAccountEmailVerifySender` - 邮件验证服务
* `AccountEmailSender` - 邮件发送实现

## 短信服务

提供以下短信服务：
* `IAccountSmsSecurityCodeSender` - 短信验证码发送服务
* `AccountSmsSecurityCodeSender` - 短信验证码发送实现

## URL配置

模块预配置了以下URL：
* EmailConfirm - 邮件确认URL，默认为"Account/EmailConfirm"
