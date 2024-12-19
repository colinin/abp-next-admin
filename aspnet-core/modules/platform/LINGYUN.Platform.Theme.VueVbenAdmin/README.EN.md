# LINGYUN.Platform.Theme.VueVbenAdmin

Theme management module for the VueVbenAdmin frontend framework, providing management functionality for theme, layout, menu, and related configurations.

## Features

* Theme Settings
  * Dark mode
  * Gray mode
  * Color weak mode
  * Theme color configuration

* Layout Settings
  * Full screen mode
  * Content mode
  * Page loading state
  * Footer display
  * Back to top

* Menu Settings
  * Menu mode
  * Menu theme
  * Menu width
  * Menu collapse
  * Menu split
  * Menu drag

* Header Settings
  * Fixed header
  * Header theme
  * Full screen button
  * Document button
  * Notice button
  * Search button

* Multi-tab Settings
  * Tab cache
  * Tab drag
  * Tab refresh
  * Tab fold

## Quick Start

1. Reference the module
```csharp
[DependsOn(typeof(PlatformThemeVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. Configure options
```json
{
  "Theme": {
    "DarkMode": false,
    "GrayMode": false,
    "ColorWeak": false,
    "ThemeColor": "#0960bd"
  },
  "Layout": {
    "FullContent": false,
    "ContentMode": "full",
    "ShowLogo": true,
    "ShowFooter": true,
    "ShowBreadCrumb": true,
    "ShowBreadCrumbIcon": false
  },
  "Menu": {
    "Mode": "inline",
    "Theme": "dark",
    "Width": 210,
    "Collapsed": false,
    "Split": false,
    "Draggable": true
  },
  "Header": {
    "Fixed": true,
    "Theme": "light",
    "ShowFullScreen": true,
    "ShowDoc": true,
    "ShowNotice": true,
    "ShowSearch": true
  },
  "MultiTab": {
    "Cache": true,
    "Draggable": true,
    "Refresh": true,
    "Fold": true
  }
}
```

## Usage Guide

1. Theme Configuration
   * Support for multiple theme mode switching
   * Customizable theme colors
   * Theme persistence storage

2. Layout Configuration
   * Flexible layout modes
   * Configurable page element display
   * Responsive layout support

3. Menu Configuration
   * Various menu display modes
   * Support for menu drag sorting
   * Menu theme customization

4. Extension Development
   * Support for custom theme configuration providers
   * Extensible theme settings
   * Theme data localization support

## Important Notes

1. Theme Configuration
   * Theme configurations affect the interface display for all users
   * Choose appropriate theme modes based on actual requirements

2. Performance Considerations
   * Configure tab cache quantity reasonably
   * Use page loading animations appropriately

## More Information

* [VueVbenAdmin Theme Configuration Documentation](https://doc.vvbin.cn/guide/design.html)
* [ABP Official Documentation](https://docs.abp.io/)
