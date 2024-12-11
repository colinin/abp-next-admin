# LINGYUN.Abp.IdentityServer.Session

IdentityServer session management module that provides user session management and validation functionality.

## Features

* Session Validation
  * `AbpIdentitySessionUserInfoRequestValidator` - User Info Request Validator
    * Validates user session status
    * Validates access token validity
    * Validates user active status
    * Supports OpenID Connect standard

* Session Event Handling
  * `AbpIdentitySessionEventServiceHandler` - Session Event Handler
    * Handles user login success events
      * Saves session information
      * Supports multi-tenancy
      * Records client identifier
    * Handles user logout success events
      * Revokes session
    * Handles token revocation success events
      * Revokes session

* Configuration Options
  * Session Claims Configuration
    * Add SessionId claim
  * Session Login Configuration
    * Disable explicit session saving
    * Enable explicit session logout

## Module Reference

```csharp
[DependsOn(
    typeof(AbpIdentityServerSessionModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Dependencies

* `AbpIdentityServerDomainModule` - ABP IdentityServer Domain Module
* `AbpIdentityDomainModule` - ABP Identity Domain Module
* `AbpIdentitySessionModule` - ABP Identity Session Module

## Configuration and Usage

### Configure Session Options

```csharp
Configure<IdentitySessionSignInOptions>(options =>
{
    // UserLoginSuccessEvent is published by IdentityServer, no need for explicit session saving
    options.SignInSessionEnabled = false;
    // UserLoginSuccessEvent is published by user, requires explicit session logout
    options.SignOutSessionEnabled = true;
});
```

### Configure Claims Options

```csharp
Configure<AbpClaimsServiceOptions>(options =>
{
    options.RequestedClaims.Add(AbpClaimTypes.SessionId);
});
```

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Modules/Identity)

[查看中文文档](README.md)
