# LINGYUN.Abp.IdentityServer.SmsValidator

IdentityServer短信验证模块，提供基于手机号和短信验证码的身份认证功能。

## 功能特性

* 短信验证
  * `SmsTokenGrantValidator` - 短信验证授权器
    * 支持手机号验证
    * 支持短信验证码验证
    * 防暴力破解保护
    * 用户锁定检查
    * 安全日志记录
    * 事件通知

* 认证流程
  1. 用户使用手机号和短信验证码发起登录请求
  2. 验证手机号和验证码的有效性
  3. 验证用户状态（是否被锁定）
  4. 验证通过后生成访问令牌
  5. 记录安全日志和事件

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerSmsValidatorModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerDomainModule` - ABP IdentityServer领域模块

## 配置使用

### 配置短信验证

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddExtensionGrantValidator<SmsTokenGrantValidator>();
    });
}
```

### 认证请求参数

* `grant_type`: "phone_verify" (必填)
* `phone_number`: 手机号 (必填)
* `phone_verify_code`: 短信验证码 (必填)
* `scope`: 请求范围 (可选)

### 认证响应

* 认证成功:
```json
{
    "access_token": "访问令牌",
    "expires_in": 有效期,
    "token_type": "Bearer",
    "refresh_token": "刷新令牌"
}
```

* 认证失败:
```json
{
    "error": "invalid_grant",
    "error_description": "错误描述"
}
```

### 错误类型

* `invalid_grant`: 授权验证失败
  * 手机号未注册
  * 验证码无效
  * 用户被锁定
  * 参数缺失

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP Identity文档](https://docs.abp.io/en/abp/latest/Modules/Identity)

[查看英文文档](README.EN.md)
