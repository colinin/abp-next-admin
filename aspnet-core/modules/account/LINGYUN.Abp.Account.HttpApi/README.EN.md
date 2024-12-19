# LINGYUN.Abp.Account.HttpApi

The HTTP API layer of the ABP account module, providing RESTful API implementations.

[简体中文](./README.md)

## Features

* Provides HTTP API endpoints for account management
* Supports localization and multi-language
* Integrates with ABP MVC framework
* Automatic API controller registration

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Controllers

### AccountController

Provides the following HTTP API endpoints:
* POST /api/account/register - Register with phone number
* POST /api/account/register-by-wechat - Register with WeChat Mini Program
* POST /api/account/reset-password - Reset password
* POST /api/account/send-phone-register-code - Send phone registration verification code
* POST /api/account/send-phone-signin-code - Send phone login verification code
* POST /api/account/send-email-signin-code - Send email login verification code
* POST /api/account/send-phone-reset-password-code - Send phone password reset verification code

### MyProfileController

Provides the following HTTP API endpoints:
* GET /api/account/my-profile - Get personal profile
* PUT /api/account/my-profile - Update personal profile
* POST /api/account/my-profile/change-password - Change password
* POST /api/account/my-profile/change-phone-number - Change phone number
* POST /api/account/my-profile/send-phone-number-change-code - Send phone number change verification code
* POST /api/account/my-profile/change-avatar - Update avatar

### MyClaimController

Provides the following HTTP API endpoints:
* GET /api/account/my-claim - Get user claims
* PUT /api/account/my-claim - Update user claims

## Localization Configuration

The module pre-configures localization options:
* Supports AccountResource localization
* Automatically registers MVC application parts
