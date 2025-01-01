# LINGYUN.Abp.TextTemplating.Application.Contracts

## Module Description

Text templating application service contracts module, providing interface definitions and DTOs for text template management.

### Base Modules

* Volo.Abp.TextTemplating
* Volo.Abp.Ddd.Application.Contracts

### Features

* Provides application service interfaces for text template management
  * ITextTemplateDefinitionAppService - Template definition management service interface
  * ITextTemplateContentAppService - Template content management service interface
* Provides DTO definitions for text templates
  * TextTemplateDefinitionDto - Template definition DTO
  * TextTemplateContentDto - Template content DTO
  * TextTemplateDefinitionCreateDto - Create template definition DTO
  * TextTemplateDefinitionUpdateDto - Update template definition DTO
  * TextTemplateContentUpdateDto - Update template content DTO
* Provides permission definitions for text template management

### Permission Definitions

* AbpTextTemplating.TextTemplateDefinitions
  * Create - Create template definition
  * Update - Update template definition
  * Delete - Delete template definition
* AbpTextTemplating.TextTemplateContents
  * Update - Update template content
  * Delete - Delete template content

### Application Service Interfaces

* ITextTemplateDefinitionAppService
  * GetAsync - Get template definition
  * GetListAsync - Get template definition list
  * CreateAsync - Create template definition
  * UpdateAsync - Update template definition
  * DeleteAsync - Delete template definition
* ITextTemplateContentAppService
  * GetAsync - Get template content
  * UpdateAsync - Update template content
  * DeleteAsync - Delete template content
  * RestoreAsync - Restore template content

### How to Use

1. Add `AbpTextTemplatingApplicationContractsModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Inject and use template service interfaces

```csharp
public class YourService
{
    private readonly ITextTemplateDefinitionAppService _templateDefinitionAppService;

    public YourService(ITextTemplateDefinitionAppService templateDefinitionAppService)
    {
        _templateDefinitionAppService = templateDefinitionAppService;
    }

    public async Task ManageTemplateAsync()
    {
        // Get template definition list
        var templates = await _templateDefinitionAppService.GetListAsync(
            new TextTemplateDefinitionGetListInput());
    }
}
```

[查看中文](README.md)
