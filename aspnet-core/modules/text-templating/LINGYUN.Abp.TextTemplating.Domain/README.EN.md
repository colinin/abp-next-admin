# LINGYUN.Abp.TextTemplating.Domain

## Module Description

Text templating domain module, providing core functionality for text template definition and content management.

### Base Modules

* Volo.Abp.TextTemplating
* Volo.Abp.Ddd.Domain

### Features

* Provides domain entities for text template definition
  * TextTemplateDefinition - Text template definition entity
  * TextTemplateContent - Text template content entity
* Provides repository interfaces for text template definition
  * ITextTemplateDefinitionRepository - Text template definition repository interface
  * ITextTemplateContentRepository - Text template content repository interface
* Provides domain services for text template management
  * TextTemplateManager - Text template manager
  * IStaticTemplateDefinitionStore - Static template definition store
  * IDynamicTemplateDefinitionStore - Dynamic template definition store

### Domain Services

* TextTemplateManager
  * Manages creation, update, and deletion of text templates
  * Handles association between template definitions and content
  * Supports management of both static and dynamic template definitions

### Entity Properties

* TextTemplateDefinition
  * Name - Template name
  * DisplayName - Display name
  * IsLayout - Whether it is a layout template
  * Layout - Layout name
  * IsInlineLocalized - Whether inline localization is enabled
  * DefaultCultureName - Default culture name
  * LocalizationResourceName - Localization resource name
  * RenderEngine - Render engine
  * IsStatic - Whether it is a static template

### How to Use

1. Add `AbpTextTemplatingDomainModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingDomainModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use text template manager

```csharp
public class YourService
{
    private readonly TextTemplateManager _templateManager;

    public YourService(TextTemplateManager templateManager)
    {
        _templateManager = templateManager;
    }

    public async Task ManageTemplateAsync()
    {
        // Create template definition
        var template = new TextTemplateDefinition(
            Guid.NewGuid(),
            "TemplateName",
            "Template Display Name",
            renderEngine: "Razor");

        await _templateManager.CreateAsync(template);
    }
}
```

[查看中文](README.md)
