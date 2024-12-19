# LINGYUN.Platform.Settings.VueVbenAdmin

The platform settings module for the VueVbenAdmin frontend framework, providing theme, layout, menu, and other setting features.

## Features

* Theme Settings
  * Dark mode
  * Gray mode
  * Color weak mode
  * Theme color

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

## Module Reference

```csharp
[DependsOn(typeof(PlatformSettingsVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Localization Resources

* Support for Simplified Chinese
* Support for English

## Configuration

```json
{
  "Settings": {
    "DarkMode": false,
    "PageLoading": true,
    "PermissionCacheType": 1,
    "ShowSettingButton": true,
    "ShowDarkModeToggle": true,
    "SettingButtonPosition": "auto",
    "PermissionMode": "ROUTE_MAPPING",
    "SessionTimeoutProcessing": 0,
    "GrayMode": false,
    "ColorWeak": false,
    "ThemeColor": "#0960bd",
    "FullContent": false,
    "ContentMode": "full",
    "ShowLogo": true,
    "ShowFooter": true,
    "OpenKeepAlive": true,
    "LockTime": 0,
    "ShowBreadCrumb": true,
    "ShowBreadCrumbIcon": false
  }
}
```

## More

For more information, please refer to [Platform](../README.md)
