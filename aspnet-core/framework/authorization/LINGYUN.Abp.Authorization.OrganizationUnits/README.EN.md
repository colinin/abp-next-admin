# LINGYUN.Abp.Authorization.OrganizationUnits

Organization Unit Authorization Module, providing organization unit-based permission validation functionality.

## Features

* Support for organization unit-based permission validation
* Provides Organization Unit Permission Value Provider (OrganizationUnitPermissionValueProvider)
* Support for multiple organization unit permission validation
* Integration with ABP permission system
* Organization Unit Claim type extensions
* Current user organization unit query extensions

## Module Reference

```csharp
[DependsOn(typeof(AbpAuthorizationOrganizationUnitsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Configure Permission Provider
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       Configure<AbpPermissionOptions>(options =>
       {
           options.ValueProviders.Add<OrganizationUnitPermissionValueProvider>();
       });
   }
   ```

2. Get Current User's Organization Units
   ```csharp
   public class YourService
   {
       private readonly ICurrentUser _currentUser;

       public YourService(ICurrentUser currentUser)
       {
           _currentUser = currentUser;
       }

       public void YourMethod()
       {
           var organizationUnits = _currentUser.FindOrganizationUnits();
           // Process business logic with organization units
       }
   }
   ```

3. Get Organization Units from ClaimsPrincipal
   ```csharp
   public class YourService
   {
       public void YourMethod(ClaimsPrincipal principal)
       {
           var organizationUnits = principal.FindOrganizationUnits();
           // Process business logic with organization units
       }
   }
   ```

## More Resources

* [GitHub Repository](https://github.com/colinin/abp-next-admin)
* [Sample Application](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/services/LY.MicroService.Applications.Single)

[简体中文](./README.md)
