# LINGYUN.Abp.Account.Application

Implementation of ABP account module application services, providing complete account management functionality.

[简体中文](./README.md)

## Features

* Account Management Service Implementation
  * Phone number registration
  * WeChat Mini Program registration
  * Password reset
  * Verification code sending (SMS, Email)
* Profile Management
  * Basic information maintenance
  * Password modification
  * Phone number change
  * Avatar update
  * Two-factor authentication
* User Claims Management
* Email Services
  * Email confirmation
  * Email verification
* SMS Services
  * Verification code sending
* WeChat Mini Program Integration
* Virtual File System Support

## Module Dependencies

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

## Application Services

### AccountAppService

Implements `IAccountAppService` interface, providing:
* User registration (phone number, WeChat Mini Program)
* Password reset
* Verification code sending

### MyProfileAppService

Implements `IMyProfileAppService` interface, providing:
* Profile management
* Password modification
* Phone number change
* Avatar update
* Two-factor authentication management

### MyClaimAppService

Implements `IMyClaimAppService` interface, providing:
* User claims management

## Email Services

Provides the following email services:
* `IAccountEmailConfirmSender` - Email confirmation service
* `IAccountEmailVerifySender` - Email verification service
* `AccountEmailSender` - Email sending implementation

## SMS Services

Provides the following SMS services:
* `IAccountSmsSecurityCodeSender` - SMS verification code sending service
* `AccountSmsSecurityCodeSender` - SMS verification code sending implementation

## URL Configuration

The module pre-configures the following URLs:
* EmailConfirm - Email confirmation URL, defaults to "Account/EmailConfirm"
