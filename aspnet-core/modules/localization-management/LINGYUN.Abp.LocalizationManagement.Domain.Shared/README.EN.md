# LINGYUN.Abp.LocalizationManagement.Domain.Shared

The shared domain layer module for localization management, defining error codes, localization resources, constants, and other shared content.

## Features

* Defines constants related to localization management
* Defines localization error codes
* Provides localization resource files
* Supports multilingual error messages

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLocalizationManagementDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Error Codes

* Localization:001100 - Language {CultureName} already exists
* Localization:001400 - Language name {CultureName} not found or built-in language operation not allowed
* Localization:002100 - Resource {Name} already exists
* Localization:002400 - Resource name {Name} not found or built-in resource operation not allowed

## Localization Resources

The module defines the following localization resources:

* DisplayName:Enable - Enable
* DisplayName:CreationTime - Creation Time
* DisplayName:LastModificationTime - Modification Time
* DisplayName:SaveAndNext - Save & Next
* Permissions:LocalizationManagement - Localization Management
* Permissions:Language - Language Management
* Permissions:Resource - Resource Management
* Permissions:Text - Text Management
* etc...

## More Information

* [中文文档](./README.md)
