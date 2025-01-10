# LINGYUN.Abp.LocalizationManagement.Application

The application service layer implementation module for localization management, providing application service implementations for localization resource management.

## Features

* Implements language management application services
* Implements resource management application services
* Implements text management application services
* Supports AutoMapper object mapping
* Provides standardized CRUD operations

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLocalizationManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Services

* `LanguageAppService`: Language management application service
  - Create language
  - Update language
  - Delete language
  - Get language list
  - Get language details

* `ResourceAppService`: Resource management application service
  - Create resource
  - Update resource
  - Delete resource
  - Get resource list
  - Get resource details

* `TextAppService`: Text management application service
  - Create text
  - Update text
  - Delete text
  - Get text list
  - Get text details

## Permissions

All application services follow the permission requirements defined by the module. See the permission definitions in the Domain.Shared module for details.

## More Information

* [中文文档](./README.md)
