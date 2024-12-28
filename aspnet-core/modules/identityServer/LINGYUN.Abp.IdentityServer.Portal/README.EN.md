# LINGYUN.Abp.IdentityServer.Portal

IdentityServer portal authentication module that provides enterprise portal authentication functionality.

## Features

* Portal Authentication
  * `PortalGrantValidator` - Portal Grant Validator
    * Supports enterprise portal login
    * Supports multi-tenant authentication
    * Automatic tenant switching
    * Enterprise information validation
    * User password validation
    * Security log recording

* Authentication Flow
  1. User initiates login request using portal
  2. Check if enterprise identifier (EnterpriseId) is provided
     * Without EnterpriseId: Returns list of enterprises with tenant information
     * With EnterpriseId: Retrieves associated tenant information and switches to specified tenant
  3. Performs login validation using password method
  4. Returns token upon successful login

## Module Reference

```csharp
[DependsOn(
    typeof(AbpIdentityServerPortalModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Dependencies

* `AbpIdentityServerDomainModule` - ABP IdentityServer Domain Module
* `AbpAspNetCoreMultiTenancyModule` - ABP Multi-tenancy Module
* `PlatformDomainModule` - Platform Domain Module

## Configuration and Usage

### Configure Portal Authentication

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddExtensionGrantValidator<PortalGrantValidator>();
    });
}
```

### Authentication Request Parameters

* `grant_type`: "portal" (required)
* `enterpriseId`: Enterprise identifier (optional)
* `username`: Username (required)
* `password`: Password (required)
* `scope`: Request scope (optional)

### Authentication Response

* When enterpriseId is not provided:
```json
{
    "error": "invalid_grant",
    "enterprises": [
        {
            "id": "enterprise_id",
            "name": "enterprise_name",
            "code": "enterprise_code"
        }
    ]
}
```

* On successful authentication:
```json
{
    "access_token": "access_token",
    "expires_in": expiration_time,
    "token_type": "Bearer",
    "refresh_token": "refresh_token"
}
```

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP Multi-tenancy Documentation](https://docs.abp.io/en/abp/latest/Multi-Tenancy)

[查看中文文档](README.md)
