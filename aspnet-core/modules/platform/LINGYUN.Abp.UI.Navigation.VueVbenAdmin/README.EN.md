# LINGYUN.Abp.UI.Navigation.VueVbenAdmin

Navigation menu initialization module for **abp-vue-vben-admin**. This module provides menu data initialization functionality required for integration with the VueVbenAdmin frontend framework.

## Features

* Menu Data Initialization
  * Preset menu structure
  * Automatic layout registration
  * Dynamic menu configuration

* VueVbenAdmin Integration
  * Layout component configuration
  * UI theme adaptation
  * Menu item customization

* Extensibility Support
  * Custom menu providers
  * Menu data override
  * Layout configuration extensions

## Quick Start

1. Reference the module
```csharp
[DependsOn(typeof(AbpUINavigationVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. Configure options
```csharp
Configure<AbpUINavigationVueVbenAdminOptions>(options =>
{
    // UI name configuration
    options.UI = "Vue Vben Admin";
    // Layout name configuration
    options.LayoutName = "Vben Admin Layout";
    // Layout component configuration
    options.LayoutPath = "LAYOUT";
});
```

## Configuration Options

* `AbpUINavigationVueVbenAdminOptions.UI`
  * Description: UI name
  * Default value: Vue Vben Admin
  * Note: Not recommended to change, must be consistent with frontend

* `AbpUINavigationVueVbenAdminOptions.LayoutName`
  * Description: Layout name
  * Default value: Vben Admin Layout
  * Note: Not recommended to change, must be consistent with frontend

* `AbpUINavigationVueVbenAdminOptions.LayoutPath`
  * Description: Layout component path
  * Default value: LAYOUT
  * Note: Not recommended to change, must be consistent with frontend

## Usage Guide

1. Menu Initialization
   * The module automatically registers default menu data
   * Custom menus can be added by implementing the `INavigationDataSeedContributor` interface

2. Layout Configuration
   * Layout configuration must match the frontend route configuration
   * When modifying layout configuration, related frontend configurations need to be synchronized

3. Extension Development
   * Custom menu initialization logic can be implemented by inheriting the `VueVbenAdminNavigationDataSeeder` class
   * Default implementations can be overridden through dependency injection

## More Information

* [VueVbenAdmin Official Documentation](https://doc.vvbin.cn/)
* [ABP Official Documentation](https://docs.abp.io/)
