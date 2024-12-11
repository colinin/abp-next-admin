# LINGYUN.Abp.IdentityServer.WeChat.Work

IdentityServer WeChat Work authentication module that provides identity authentication functionality based on WeChat Work.

## Features

* WeChat Work Authentication
  * `WeChatWorkGrantValidator` - WeChat Work Grant Validator
    * WeChat Work login support
    * Multi-tenant support
    * Automatic user registration
    * Security log recording
    * Event notifications
    * Localization support

* Authentication Flow
  1. User initiates login request through WeChat Work
  2. Validates AgentId and Code
  3. Retrieves WeChat Work user information
  4. Verifies user registration status
     * Direct login for registered users
     * Automatic registration based on configuration for unregistered users
  5. Generates access token
  6. Records security logs and events

## Module Reference

```csharp
[DependsOn(
    typeof(AbpIdentityServerWeChatWorkModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Dependencies

* `AbpIdentityServerDomainModule` - ABP IdentityServer Domain Module
* `AbpWeChatWorkModule` - ABP WeChat Work Module

## Configuration and Usage

### Configure WeChat Work Authentication

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddExtensionGrantValidator<WeChatWorkGrantValidator>();
    });
}
```

### Authentication Request Parameters

* `grant_type`: "wechat_work" (required)
* `agent_id`: WeChat Work application ID (required)
* `code`: WeChat Work authorization code (required)
* `scope`: Request scope (optional)

### Authentication Response

* On successful authentication:
```json
{
    "access_token": "access_token",
    "expires_in": expiration_time,
    "token_type": "Bearer",
    "refresh_token": "refresh_token"
}
```

* On authentication failure:
```json
{
    "error": "invalid_grant",
    "error_description": "error description"
}
```

### Configuration Options

* Quick Login
```csharp
Configure<AbpSettingOptions>(options =>
{
    // Enable quick login for unregistered users
    options.SetDefault(WeChatWorkSettingNames.EnabledQuickLogin, true);
});
```

### Error Types

* `invalid_grant`: Grant validation failed
  * Invalid AgentId or Code
  * User not registered and quick login not enabled
  * WeChat Work API call failed

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [WeChat Work API Documentation](https://work.weixin.qq.com/api/doc)

[查看中文文档](README.md)
