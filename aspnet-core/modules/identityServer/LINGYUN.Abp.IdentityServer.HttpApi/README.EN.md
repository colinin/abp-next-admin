# LINGYUN.Abp.IdentityServer.HttpApi

IdentityServer HTTP API module, providing HTTP API interfaces for IdentityServer4 resource management.

## Features

* API Controllers
  * API Scope Controller - `ApiScopeController`
    * Create API Scope - POST `/api/identity-server/api-scopes`
    * Delete API Scope - DELETE `/api/identity-server/api-scopes/{id}`
    * Get API Scope - GET `/api/identity-server/api-scopes/{id}`
    * Get API Scope List - GET `/api/identity-server/api-scopes`
    * Update API Scope - PUT `/api/identity-server/api-scopes/{id}`

  * API Resource Controller - `ApiResourceController`
    * Provides CRUD operation interfaces for API resources
    * Route prefix: `/api/identity-server/api-resources`

* Localization Support
  * Inherits ABP UI resource localization configuration
  * Supports multiple languages

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityServerHttpApiModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Required Modules

* `AbpIdentityServerApplicationContractsModule` - IdentityServer Application Contracts Module
* `AbpAspNetCoreMvcModule` - ABP ASP.NET Core MVC Module

## Configuration and Usage

### Configure Remote Service Name

```csharp
[RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
[Area("identity-server")]
[Route("api/identity-server/[controller]")]
public class YourController : AbpControllerBase
{
    // ...
}
```

### Add Localization Resource

```csharp
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<AbpIdentityServerResource>()
        .AddBaseTypes(typeof(AbpUiResource));
});
```

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP ASP.NET Core MVC Documentation](https://docs.abp.io/en/abp/latest/AspNetCore-MVC)

[查看中文文档](README.md)
