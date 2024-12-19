# LINGYUN.Abp.IdentityServer.LinkUser

IdentityServer user linking module, providing support for user linking extension grant type.

## Features

* Extension Grant Validator
  * `LinkUserGrantValidator` - User Linking Grant Validator
    * Grant Type: `link_user`
    * Supports access token validation
    * Supports user linking relationship validation
    * Supports multi-tenant scenarios
    * Supports custom claims extension

* Localization Support
  * Built-in Chinese and English resources
  * Support for extending other languages

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityServerLinkUserModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Required Modules

* `AbpIdentityServerDomainModule` - ABP IdentityServer Domain Module

## Configuration and Usage

### Authorization Request Parameters

* `grant_type` - Must be `link_user`
* `access_token` - Current user's access token
* `LinkUserId` - Target user ID to link
* `LinkTenantId` - Target user's tenant ID (optional)

### Authorization Request Example

```http
POST /connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=link_user&
access_token=current_user_access_token&
LinkUserId=target_user_id&
LinkTenantId=target_tenant_id
```

### Custom Claims Extension

```csharp
public class CustomLinkUserGrantValidator : LinkUserGrantValidator
{
    protected override Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user, ExtensionGrantValidationContext context)
    {
        // Add custom claims
        customClaims.Add(new Claim("custom_claim", "custom_value"));

        return base.AddCustomClaimsAsync(customClaims, user, context);
    }
}
```

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP Authentication Documentation](https://docs.abp.io/en/abp/latest/Authentication)

[查看中文文档](README.md)
