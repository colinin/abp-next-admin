# LINGYUN.Abp.UI.Navigation

[简体中文](./README.md) | English

Menu navigation module that provides extensible custom menu items.

## Features

* Support for custom menu item definitions and extensions
* Multi-tenancy support
* Support for menu item hierarchy
* Support for menu item ordering
* Support for menu item visibility and disabled state
* Support for menu item icons and redirects
* Support for menu seed data initialization

## Module Dependencies

```csharp
[DependsOn(typeof(AbpUINavigationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

### 1. Define Menu Provider

Implement the `INavigationDefinitionProvider` interface to define menu items:

```csharp
public class FakeNavigationDefinitionProvider : NavigationDefinitionProvider
{
    public override void Define(INavigationDefinitionContext context)
    {
        context.Add(GetNavigationDefinition());
    }

    private static NavigationDefinition GetNavigationDefinition()
    {
        var dashboard = new ApplicationMenu(
            name: "Vben Dashboard",
            displayName: "Dashboard",
            url: "/dashboard",
            component: "",
            description: "Dashboard",
            icon: "ion:grid-outline",
            redirect: "/dashboard/analysis");

         dashboard.AddItem(
            new ApplicationMenu(
            name: "Analysis",
            displayName: "Analysis Page",
            url: "/dashboard/analysis",
            component: "/dashboard/analysis/index",
            description: "Analysis Page"));

         return new NavigationDefinition(dashboard);
    }
}
```

### 2. Initialize Menu Data

Implement the `INavigationSeedContributor` interface to initialize menu seed data:

```csharp
public class YourNavigationDataSeeder : INavigationSeedContributor
{
    public async Task SeedAsync(NavigationSeedContext context)
    {
        // Initialize menu data here
    }
}
```

## Menu Item Properties

* Name - Menu item name (unique identifier)
* DisplayName - Display name
* Description - Description
* Url - Path
* Component - Component path
* Redirect - Redirect path
* Icon - Icon
* Order - Order (default 1000)
* IsDisabled - Whether disabled
* IsVisible - Whether visible
* Items - Child menu items collection
* ExtraProperties - Extension properties
* MultiTenancySides - Multi-tenancy options

## Best Practices

1. Use meaningful names for menu items for better management and maintenance
2. Use menu item ordering appropriately to keep the interface clean
3. Use icons to enhance user experience
4. Use redirect functionality appropriately to optimize navigation experience
5. Control menu item visibility and disabled state based on actual requirements

## Notes

1. Menu item Name must be unique
2. Child menu items inherit multi-tenant settings from parent menu items
3. Menu items with smaller order values appear first
4. Disabled menu items are still visible but cannot be clicked
5. Component paths should be consistent with front-end routing configuration
