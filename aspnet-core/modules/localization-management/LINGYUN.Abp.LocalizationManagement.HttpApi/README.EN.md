# LINGYUN.Abp.LocalizationManagement.HttpApi

The HTTP API module for localization management, providing RESTful API interfaces.

## Features

* Provides HTTP API interfaces for localization management
* Supports REST operations for languages, resources, and texts
* Integrates ABP dynamic API functionality
* Supports API version control
* Supports Swagger documentation generation

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLocalizationManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API Controllers

* `LanguageController`: Language management API controller
* `ResourceController`: Resource management API controller
* `TextController`: Text management API controller

## Configuration

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
    {
        options.AddAssemblyResource(
            typeof(LocalizationManagementResource),
            typeof(AbpLocalizationManagementApplicationContractsModule).Assembly);
    });

    PreConfigure<IMvcBuilder>(mvcBuilder =>
    {
        mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpLocalizationManagementApplicationContractsModule).Assembly);
    });
}
```

## API Routes

* `/api/localization-management/languages` - Language management related APIs
* `/api/localization-management/resources` - Resource management related APIs
* `/api/localization-management/texts` - Text management related APIs

## More Information

* [中文文档](./README.md)
