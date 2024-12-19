# LINGYUN.Abp.AspNetCore.Mvc.Localization

ABP framework localization management module, providing Web API interfaces for localization resources.

## Features

* Provides localization text management API interfaces
* Supports multi-language resource querying and comparison
* Supports filtering by resource name and culture name
* Supports integration of external localization resources
* Supports localization resource difference comparison

## Installation

```bash
dotnet add package LINGYUN.Abp.AspNetCore.Mvc.Localization
```

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcLocalizationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Endpoints

### Language Management

* GET /api/localization/languages - Get all available languages
* GET /api/localization/languages/{cultureName} - Get specific language information

### Resource Management

* GET /api/localization/resources - Get all localization resources
* GET /api/localization/resources/{name} - Get specific localization resource

### Text Management

* GET /api/localization/texts - Get localization text list
* GET /api/localization/texts/by-key - Get localization text by key
* GET /api/localization/texts/differences - Get text differences between languages

## Basic Usage

1. Configure localization resources
```csharp
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<YourLocalizationResource>()
        .AddVirtualJson("/Your/Resource/Path");
});
```

2. Inject and use the service
```csharp
public class YourService
{
    private readonly ITextAppService _textAppService;
    
    public YourService(ITextAppService textAppService)
    {
        _textAppService = textAppService;
    }
    
    public async Task GetLocalizedText(string key, string cultureName)
    {
        var text = await _textAppService.GetByCultureKeyAsync(new GetTextByKeyInput
        {
            Key = key,
            CultureName = cultureName,
            ResourceName = "YourResourceName"
        });
        // Use the localized text
    }
}
```

## More Information

* [中文文档](./README.md)
* [ABP Localization Documentation](https://docs.abp.io/en/abp/latest/Localization)
