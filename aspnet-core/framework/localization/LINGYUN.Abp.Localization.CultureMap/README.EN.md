# LINGYUN.Abp.Localization.CultureMap

## Module Description

This module solves localization issues with multiple culture format variants. It allows you to map different culture format identifiers to a standard format.

Reference Project: [Owl.Abp.CultureMap](https://github.com/maliming/Owl.Abp.CultureMap)

## Features

* Support mapping multiple culture format identifiers to standard format
* Support independent mapping for Culture and UICulture
* Integration with ABP request localization
* Support custom culture mapping rules

## Installation

```bash
dotnet add package LINGYUN.Abp.Localization.CultureMap
```

## Base Modules

* Volo.Abp.AspNetCore

## Configuration

The module provides the following configuration options:

* CulturesMaps: List of culture mappings
* UiCulturesMaps: List of UI culture mappings

Each mapping item contains:
* TargetCulture: Target culture identifier
* SourceCultures: List of source culture identifiers

## Usage

1. Add module dependency:

```csharp
[DependsOn(
    typeof(AbpLocalizationCultureMapModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationCultureMapOptions>(options =>
        {
            var zhHansCultureMapInfo = new CultureMapInfo
            {
                TargetCulture = "zh-Hans",
                SourceCultures = new string[] { "zh", "zh_CN", "zh-CN" }
            };

            options.CulturesMaps.Add(zhHansCultureMapInfo);
            options.UiCulturesMaps.Add(zhHansCultureMapInfo);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseMapRequestLocalization();
    }
}
```

## Common Use Cases

1. Unify Simplified Chinese culture identifiers:
```csharp
options.CulturesMaps.Add(new CultureMapInfo
{
    TargetCulture = "zh-Hans",
    SourceCultures = new string[] { "zh", "zh_CN", "zh-CN" }
});
```

2. Unify Traditional Chinese culture identifiers:
```csharp
options.CulturesMaps.Add(new CultureMapInfo
{
    TargetCulture = "zh-Hant",
    SourceCultures = new string[] { "zh_TW", "zh-TW", "zh_HK", "zh-HK" }
});
```

## More Information

* [中文文档](./README.md)
* [ABP Localization Documentation](https://docs.abp.io/en/abp/latest/Localization)
