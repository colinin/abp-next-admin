# LINGYUN.Abp.Identity.HttpApi

Identity authentication HTTP API module, providing HTTP API interfaces for identity authentication.

## Features

* Extends Volo.Abp.Identity.AbpIdentityHttpApiModule module
* Provides HTTP API interfaces for identity authentication
* Supports MVC data annotations with localization resources
* Automatically registers MVC application parts

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Controllers

* IdentityUserController - User management API
* IdentityRoleController - Role management API
* IdentityClaimTypeController - Claim type management API
* IdentitySecurityLogController - Security log API
* IdentitySettingsController - Identity settings API
* ProfileController - User profile API

## API Routes

* `/api/identity/users` - User management API routes
* `/api/identity/roles` - Role management API routes
* `/api/identity/claim-types` - Claim type management API routes
* `/api/identity/security-logs` - Security log API routes
* `/api/identity/settings` - Identity settings API routes
* `/api/identity/my-profile` - User profile API routes

## Basic Usage

1. Configure localization
```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
    {
        options.AddAssemblyResource(typeof(IdentityResource), typeof(AbpIdentityApplicationContractsModule).Assembly);
    });
}
```

2. Use APIs
```csharp
// Get user list
GET /api/identity/users

// Get specific user
GET /api/identity/users/{id}

// Create user
POST /api/identity/users
{
    "userName": "admin",
    "email": "admin@abp.io",
    "password": "1q2w3E*"
}

// Update user
PUT /api/identity/users/{id}
{
    "userName": "admin",
    "email": "admin@abp.io"
}

// Delete user
DELETE /api/identity/users/{id}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [ABP HTTP API Documentation](https://docs.abp.io/en/abp/latest/API/HTTP-API)
