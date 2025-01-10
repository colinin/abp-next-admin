# LINGYUN.Abp.Account.Application.Contracts

ABP账户模块的应用服务契约，提供账户管理相关的接口定义。

[English](./README.EN.md)

## 功能特性

* 手机号注册账户
* 微信小程序注册账户
* 手机号重置密码
* 手机验证码功能（注册、登录、重置密码）
* 邮箱验证码登录
* 用户个人资料管理
* 用户会话管理
* 双因素认证
* 用户声明管理

## 模块引用

```csharp
[DependsOn(typeof(AbpAccountApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 接口服务

### IAccountAppService

账户管理服务接口，提供以下功能：
* 手机号注册
* 微信小程序注册
* 手机号重置密码
* 发送手机验证码（注册、登录、重置密码）
* 发送邮箱验证码（登录）

### IMyProfileAppService

个人资料管理服务接口，提供以下功能：
* 获取/更新个人资料
* 更改密码
* 更改手机号
* 更改头像
* 双因素认证管理
* 获取/验证认证器
* 获取恢复代码

### IMyClaimAppService

用户声明管理服务接口，提供以下功能：
* 获取用户声明
* 更新用户声明

## 本地化

模块包含多语言支持，资源文件位于：
* `/LINGYUN/Abp/Account/Localization/Resources`
