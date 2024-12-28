# LINGYUN.Abp.Identity.OrganizaztionUnits

Identity authentication organization units module, providing integration functionality between the identity authentication system and organization units.

## Features

* Extends AbpIdentityDomainModule module
* Integrates AbpAuthorizationOrganizationUnitsModule module
* Supports dynamic addition of organization unit claims
* Provides organization unit-related identity authentication functionality

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpAuthorizationOrganizationUnitsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

### AbpClaimsPrincipalFactoryOptions

```json
{
  "Abp": {
    "Security": {
      "Claims": {
        "DynamicClaims": {
          "OrganizationUnit": true  // Enable organization unit dynamic claims
        }
      }
    }
  }
}
```

## Claim Types

* `AbpOrganizationUnitClaimTypes.OrganizationUnit` - Organization unit claim type
  * Used to identify the organization unit a user belongs to
  * Automatically added to claims during user authentication

## Basic Usage

1. Configure organization unit claims
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpClaimsPrincipalFactoryOptions>(options =>
    {
        options.DynamicClaims.Add(AbpOrganizationUnitClaimTypes.OrganizationUnit);
    });
}
```

2. Get user's organization unit claims
```csharp
public class YourService
{
    private readonly ICurrentUser _currentUser;

    public YourService(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string[] GetUserOrganizationUnits()
    {
        return _currentUser.FindClaims(AbpOrganizationUnitClaimTypes.OrganizationUnit)
            .Select(c => c.Value)
            .ToArray();
    }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [ABP Organization Units Documentation](https://docs.abp.io/en/abp/latest/Organization-Units)
