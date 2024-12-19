# LINGYUN.Abp.TextTemplating.Domain.Shared

## Module Description

Text templating domain shared module, providing shared definitions for text templates including constants, enums, and exceptions.

### Base Modules

* Volo.Abp.TextTemplating
* Volo.Abp.Validation

### Features

* Provides constants for text templates
  * TextTemplateDefinitionConsts - Template definition related constants
  * TextTemplateContentConsts - Template content related constants
* Provides error code definitions
  * AbpTextTemplatingErrorCodes - Error code constants
* Provides localization resources
  * AbpTextTemplatingResource - Localization resource

### Constants

* TextTemplateDefinitionConsts
  * MaxNameLength - Maximum length for template name (64)
  * MaxDisplayNameLength - Maximum length for display name (128)
  * MaxLayoutLength - Maximum length for layout name (256)
  * MaxDefaultCultureNameLength - Maximum length for default culture name (10)
  * MaxLocalizationResourceNameLength - Maximum length for localization resource name (128)
  * MaxRenderEngineLength - Maximum length for render engine name (64)

### Error Codes

* AbpTextTemplatingErrorCodes
  * TextTemplateDefinition:NameAlreadyExists - Template name already exists
  * TextTemplateDefinition:NotFound - Template definition not found

### How to Use

1. Add `AbpTextTemplatingDomainSharedModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingDomainSharedModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use constants and error codes

```csharp
public class YourService
{
    public void ValidateTemplateName(string name)
    {
        if (name.Length > TextTemplateDefinitionConsts.MaxNameLength)
        {
            throw new BusinessException(AbpTextTemplatingErrorCodes.TextTemplateDefinition.NameAlreadyExists);
        }
    }
}
```

[查看中文](README.md)
