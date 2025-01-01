# LINGYUN.Abp.TextTemplating.Application

## Module Description

Text templating application service module, implementing management and operation functions for text templates.

### Base Modules

* LINGYUN.Abp.TextTemplating.Application.Contracts
* Volo.Abp.TextTemplating
* Volo.Abp.Ddd.Application

### Features

* Provides text template definition management services
  * TextTemplateDefinitionAppService - Template definition management service
  * TextTemplateContentAppService - Template content management service
* Implements the following application service interfaces
  * ITextTemplateDefinitionAppService
  * ITextTemplateContentAppService

### Application Services

* TextTemplateDefinitionAppService
  * GetAsync - Get template definition
  * GetListAsync - Get template definition list
  * CreateAsync - Create template definition
  * UpdateAsync - Update template definition
  * DeleteAsync - Delete template definition
* TextTemplateContentAppService
  * GetAsync - Get template content
  * UpdateAsync - Update template content
  * DeleteAsync - Delete template content
  * RestoreAsync - Restore template content

### Permissions

* AbpTextTemplating.TextTemplateDefinitions
  * Create - Create template definition
  * Update - Update template definition
  * Delete - Delete template definition
* AbpTextTemplating.TextTemplateContents
  * Update - Update template content
  * Delete - Delete template content

### How to Use

1. Add `AbpTextTemplatingApplicationModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingApplicationModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Inject and use template services

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
        // Create template definition
        var template = await _templateDefinitionAppService.CreateAsync(
            new TextTemplateDefinitionCreateDto
            {
                Name = "TemplateName",
                DisplayName = "Template Display Name",
                RenderEngine = "Razor"
            });
    }
}
```

[查看中文](README.md)
