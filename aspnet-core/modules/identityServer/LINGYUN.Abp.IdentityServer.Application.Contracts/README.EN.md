# LINGYUN.Abp.IdentityServer.Application.Contracts

IdentityServer application service contracts module, defining application service interfaces and DTOs for IdentityServer4 resource management functionality.

## Features

* Permission Definitions
  * Client Permissions
    * Default Permission - `AbpIdentityServer.Clients`
    * Create Permission - `AbpIdentityServer.Clients.Create`
    * Update Permission - `AbpIdentityServer.Clients.Update`
    * Delete Permission - `AbpIdentityServer.Clients.Delete`
    * Clone Permission - `AbpIdentityServer.Clients.Clone`
    * Manage Permissions - `AbpIdentityServer.Clients.ManagePermissions`
    * Manage Claims - `AbpIdentityServer.Clients.ManageClaims`
    * Manage Secrets - `AbpIdentityServer.Clients.ManageSecrets`
    * Manage Properties - `AbpIdentityServer.Clients.ManageProperties`

  * API Resource Permissions
    * Default Permission - `AbpIdentityServer.ApiResources`
    * Create Permission - `AbpIdentityServer.ApiResources.Create`
    * Update Permission - `AbpIdentityServer.ApiResources.Update`
    * Delete Permission - `AbpIdentityServer.ApiResources.Delete`
    * Manage Claims - `AbpIdentityServer.ApiResources.ManageClaims`
    * Manage Secrets - `AbpIdentityServer.ApiResources.ManageSecrets`
    * Manage Scopes - `AbpIdentityServer.ApiResources.ManageScopes`
    * Manage Properties - `AbpIdentityServer.ApiResources.ManageProperties`

  * API Scope Permissions
    * Default Permission - `AbpIdentityServer.ApiScopes`
    * Create Permission - `AbpIdentityServer.ApiScopes.Create`
    * Update Permission - `AbpIdentityServer.ApiScopes.Update`
    * Delete Permission - `AbpIdentityServer.ApiScopes.Delete`
    * Manage Claims - `AbpIdentityServer.ApiScopes.ManageClaims`
    * Manage Properties - `AbpIdentityServer.ApiScopes.ManageProperties`

  * Identity Resource Permissions
    * Default Permission - `AbpIdentityServer.IdentityResources`
    * Create Permission - `AbpIdentityServer.IdentityResources.Create`
    * Update Permission - `AbpIdentityServer.IdentityResources.Update`
    * Delete Permission - `AbpIdentityServer.IdentityResources.Delete`
    * Manage Claims - `AbpIdentityServer.IdentityResources.ManageClaims`
    * Manage Properties - `AbpIdentityServer.IdentityResources.ManageProperties`

  * Grant Permissions
    * Default Permission - `AbpIdentityServer.Grants`
    * Delete Permission - `AbpIdentityServer.Grants.Delete`

* Localization Resources
  * Support for multi-language localization
  * Built-in Chinese and English resources

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityServerApplicationContractsModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Required Modules

* `AbpAuthorizationModule` - ABP Authorization Module
* `AbpDddApplicationContractsModule` - ABP DDD Application Contracts Module
* `AbpIdentityServerDomainSharedModule` - IdentityServer Domain Shared Module

## Configuration and Usage

The module provides application service interface definitions and data transfer objects required for IdentityServer4 resource management. All permissions are by default only available to the host tenant.

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP Authorization Documentation](https://docs.abp.io/en/abp/latest/Authorization)

[查看中文文档](README.md)
