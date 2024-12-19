# LINGYUN.Abp.TextTemplating.EntityFrameworkCore

## Module Description

Text templating EntityFrameworkCore module, providing data access implementation for text templates.

### Base Modules

* LINGYUN.Abp.TextTemplating.Domain
* Volo.Abp.EntityFrameworkCore

### Features

* Implements repository interfaces for text templates
  * EfCoreTextTemplateDefinitionRepository - Template definition repository implementation
  * EfCoreTextTemplateContentRepository - Template content repository implementation
* Provides database context and configuration
  * ITextTemplatingDbContext - Text templating database context interface
  * TextTemplatingDbContext - Text templating database context
  * TextTemplatingDbContextModelCreatingExtensions - Database model configuration extensions

### Database Tables

* AbpTextTemplateDefinitions - Template definition table
  * Id - Primary key
  * Name - Template name
  * DisplayName - Display name
  * IsLayout - Whether it is a layout template
  * Layout - Layout name
  * IsInlineLocalized - Whether inline localization is enabled
  * DefaultCultureName - Default culture name
  * LocalizationResourceName - Localization resource name
  * RenderEngine - Render engine
  * IsStatic - Whether it is a static template
* AbpTextTemplateContents - Template content table
  * Id - Primary key
  * Name - Template name
  * CultureName - Culture name
  * Content - Template content

### How to Use

1. Add `AbpTextTemplatingEntityFrameworkCoreModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Configure database context

```csharp
public class YourDbContext : AbpDbContext<YourDbContext>, ITextTemplatingDbContext
{
    public DbSet<TextTemplateDefinition> TextTemplateDefinitions { get; set; }
    public DbSet<TextTemplateContent> TextTemplateContents { get; set; }

    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTextTemplating();
    }
}
```

[查看中文](README.md)
