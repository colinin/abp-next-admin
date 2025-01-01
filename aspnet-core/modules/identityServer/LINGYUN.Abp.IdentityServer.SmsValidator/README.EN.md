# LINGYUN.Abp.IdentityServer.SmsValidator

IdentityServer SMS verification module that provides authentication functionality based on phone numbers and SMS verification codes.

## Features

* SMS Verification
  * `SmsTokenGrantValidator` - SMS Token Grant Validator
    * Phone number validation
    * SMS verification code validation
    * Brute force protection
    * User lockout check
    * Security log recording
    * Event notifications

* Authentication Flow
  1. User initiates login request with phone number and SMS verification code
  2. Validates phone number and verification code
  3. Checks user status (whether locked)
  4. Generates access token upon successful validation
  5. Records security logs and events

## Module Reference

```csharp
[DependsOn(
    typeof(AbpIdentityServerSmsValidatorModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Dependencies

* `AbpIdentityServerDomainModule` - ABP IdentityServer Domain Module

## Configuration and Usage

### Configure SMS Validation

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddExtensionGrantValidator<SmsTokenGrantValidator>();
    });
}
```

### Authentication Request Parameters

* `grant_type`: "phone_verify" (required)
* `phone_number`: Phone number (required)
* `phone_verify_code`: SMS verification code (required)
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

### Error Types

* `invalid_grant`: Grant validation failed
  * Phone number not registered
  * Invalid verification code
  * User locked out
  * Missing parameters

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Modules/Identity)

[查看中文文档](README.md)
