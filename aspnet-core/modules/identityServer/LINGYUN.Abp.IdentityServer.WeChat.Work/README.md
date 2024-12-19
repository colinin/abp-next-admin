# LINGYUN.Abp.IdentityServer.WeChat.Work

IdentityServer企业微信认证模块，提供基于企业微信的身份认证功能。

## 功能特性

* 企业微信认证
  * `WeChatWorkGrantValidator` - 企业微信授权验证器
    * 支持企业微信登录
    * 支持多租户
    * 自动用户注册
    * 安全日志记录
    * 事件通知
    * 本地化支持

* 认证流程
  1. 用户通过企业微信发起登录请求
  2. 验证AgentId和Code的有效性
  3. 获取企业微信用户信息
  4. 验证用户注册状态
     * 已注册用户直接登录
     * 未注册用户根据配置自动注册
  5. 生成访问令牌
  6. 记录安全日志和事件

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerWeChatWorkModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `AbpIdentityServerDomainModule` - ABP IdentityServer领域模块
* `AbpWeChatWorkModule` - ABP企业微信模块

## 配置使用

### 配置企业微信认证

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddExtensionGrantValidator<WeChatWorkGrantValidator>();
    });
}
```

### 认证请求参数

* `grant_type`: "wechat_work" (必填)
* `agent_id`: 企业微信应用ID (必填)
* `code`: 企业微信授权码 (必填)
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

### 配置选项

* 快速登录
```csharp
Configure<AbpSettingOptions>(options =>
{
    // 启用未注册用户快速登录
    options.SetDefault(WeChatWorkSettingNames.EnabledQuickLogin, true);
});
```

### 错误类型

* `invalid_grant`: 授权验证失败
  * AgentId或Code无效
  * 用户未注册且未启用快速登录
  * 企业微信API调用失败

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [企业微信开发文档](https://work.weixin.qq.com/api/doc)

[查看英文文档](README.EN.md)
