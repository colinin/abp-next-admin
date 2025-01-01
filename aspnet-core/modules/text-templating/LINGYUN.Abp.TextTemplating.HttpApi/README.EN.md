# LINGYUN.Abp.TextTemplating.HttpApi

## Module Description

Text templating HTTP API module, providing RESTful API interfaces for text template management.

### Base Modules

* LINGYUN.Abp.TextTemplating.Application.Contracts
* Volo.Abp.AspNetCore.Mvc

### Features

* Provides API controllers for text template management
  * TextTemplateDefinitionController - Template definition management controller
  * TextTemplateContentController - Template content management controller

### API Endpoints

* /api/text-templating/template-definitions
  * GET - Get template definition list
  * POST - Create template definition
  * PUT - Update template definition
  * DELETE - Delete template definition
  * GET /{name} - Get specific template definition
* /api/text-templating/template-contents
  * GET - Get template content
  * PUT - Update template content
  * DELETE - Delete template content
  * POST /restore - Restore template content

### Permission Requirements

* AbpTextTemplating.TextTemplateDefinitions
  * Create - Create template definition
  * Update - Update template definition
  * Delete - Delete template definition
* AbpTextTemplating.TextTemplateContents
  * Update - Update template content
  * Delete - Delete template content

### How to Use

1. Add `AbpTextTemplatingHttpApiModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingHttpApiModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use API endpoints

```csharp
public class YourService
{
    private readonly HttpClient _httpClient;

    public YourService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ManageTemplateAsync()
    {
        // Get template definition list
        var response = await _httpClient.GetAsync("/api/text-templating/template-definitions");
        var templates = await response.Content.ReadFromJsonAsync<ListResultDto<TextTemplateDefinitionDto>>();
    }
}
```

[查看中文](README.md)
