# LINGYUN.Abp.IdentityServer.Session

IdentityServer会话管理模块，提供用户会话管理和验证功能。

## 功能特性

* 会话验证
  * `AbpIdentitySessionUserInfoRequestValidator` - 用户信息请求验证器
    * 验证用户会话状态
    * 验证访问令牌有效性
    * 验证用户活动状态
    * 支持OpenID Connect标准

* 会话事件处理
  * `AbpIdentitySessionEventServiceHandler` - 会话事件处理器
    * 处理用户登录成功事件
      * 保存会话信息
      * 支持多租户
      * 记录客户端标识
    * 处理用户登出成功事件
      * 撤销会话
    * 处理令牌撤销成功事件
      * 撤销会话

* 配置选项
  * 会话声明配置
    * 添加SessionId声明
  * 会话登录配置
    * 禁用显式保存会话
    * 启用显式注销会话

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerSessionModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerDomainModule` - ABP IdentityServer领域模块
* `AbpIdentityDomainModule` - ABP Identity领域模块
* `AbpIdentitySessionModule` - ABP Identity会话模块

## 配置使用

### 配置会话选项

```csharp
Configure<IdentitySessionSignInOptions>(options =>
{
    // UserLoginSuccessEvent由IdentityServer发布, 无需显式保存会话
    options.SignInSessionEnabled = false;
    // UserLoginSuccessEvent由用户发布, 需要显式注销会话
    options.SignOutSessionEnabled = true;
});
```

### 配置声明选项

```csharp
Configure<AbpClaimsServiceOptions>(options =>
{
    options.RequestedClaims.Add(AbpClaimTypes.SessionId);
});
```

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP Identity文档](https://docs.abp.io/en/abp/latest/Modules/Identity)

[查看英文文档](README.EN.md)
