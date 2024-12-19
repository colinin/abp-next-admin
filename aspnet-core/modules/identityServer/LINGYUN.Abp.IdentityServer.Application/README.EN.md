# LINGYUN.Abp.IdentityServer.Application

IdentityServer application service module, providing application layer implementation for IdentityServer4 resource management functionality.

## Features

* Client Management Services
  * Client Secret Management
  * Client Scope Management
  * Client Grant Type Management
  * Client CORS Origin Management
  * Client Redirect URI Management
  * Client Post-Logout Redirect URI Management
  * Client Identity Provider Restriction Management
  * Client Claim Management
  * Client Property Management

* API Resource Management Services
  * API Resource Property Management
  * API Resource Secret Management
  * API Resource Scope Management
  * API Resource Claim Management

* API Scope Management Services
  * API Scope Claim Management
  * API Scope Property Management

* Identity Resource Management Services
  * Identity Resource Claim Management
  * Identity Resource Property Management

* Persisted Grant Management Services

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityServerApplicationModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Required Modules

* `AbpIdentityServerApplicationContractsModule` - IdentityServer Application Contracts Module
* `AbpIdentityServerDomainModule` - IdentityServer Domain Module
* `AbpDddApplicationModule` - ABP DDD Application Base Module
* `AbpAutoMapperModule` - ABP AutoMapper Object Mapping Module

## Configuration and Usage

The module implements CRUD operations for IdentityServer4 resources, primarily used for managing IdentityServer4 configuration resources.

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP Authorization Documentation](https://docs.abp.io/en/abp/latest/Authorization)

[查看中文文档](README.md)
