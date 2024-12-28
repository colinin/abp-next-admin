# LINGYUN.Abp.LocalizationManagement.Application.Contracts

The application service layer contract module for localization management, defining application service interfaces, DTO objects, and permission definitions.

## Features

* Defines language management application service interfaces
* Defines resource management application service interfaces
* Defines text management application service interfaces
* Defines permissions and authorization
* Provides DTO object definitions

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLocalizationManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Permission Definitions

* LocalizationManagement.Resource - Authorizes access to resources
* LocalizationManagement.Resource.Create - Authorizes resource creation
* LocalizationManagement.Resource.Update - Authorizes resource modification
* LocalizationManagement.Resource.Delete - Authorizes resource deletion
* LocalizationManagement.Language - Authorizes access to languages
* LocalizationManagement.Language.Create - Authorizes language creation
* LocalizationManagement.Language.Update - Authorizes language modification
* LocalizationManagement.Language.Delete - Authorizes language deletion
* LocalizationManagement.Text - Authorizes access to texts
* LocalizationManagement.Text.Create - Authorizes text creation
* LocalizationManagement.Text.Update - Authorizes text modification
* LocalizationManagement.Text.Delete - Authorizes text deletion

## Application Service Interfaces

* `ILanguageAppService`: Language management application service interface
* `IResourceAppService`: Resource management application service interface
* `ITextAppService`: Text management application service interface

## More Information

* [中文文档](./README.md)
