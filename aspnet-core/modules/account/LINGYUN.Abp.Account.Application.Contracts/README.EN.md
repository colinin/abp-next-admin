# LINGYUN.Abp.Account.Application.Contracts

Application service contracts for the ABP account module, providing interface definitions for account management.

[简体中文](./README.md)

## Features

* Phone number registration
* WeChat Mini Program registration
* Password reset via phone number
* Phone verification code functionality (registration, login, password reset)
* Email verification code login
* User profile management
* User session management
* Two-factor authentication
* User claims management

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAccountApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Service Interfaces

### IAccountAppService

Account management service interface, providing:
* Phone number registration
* WeChat Mini Program registration
* Password reset via phone number
* Send phone verification codes (registration, login, password reset)
* Send email verification codes (login)

### IMyProfileAppService

Profile management service interface, providing:
* Get/Update personal profile
* Change password
* Change phone number
* Change avatar
* Two-factor authentication management
* Get/Verify authenticator
* Get recovery codes

### IMyClaimAppService

User claims management service interface, providing:
* Get user claims
* Update user claims

## Localization

The module includes multi-language support, with resource files located at:
* `/LINGYUN/Abp/Account/Localization/Resources`
