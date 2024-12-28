# LINGYUN.Abp.TextTemplating.Scriban

## Module Description

Text templating Scriban engine module, providing template rendering implementation based on Scriban syntax.

### Base Modules

* Volo.Abp.TextTemplating.Scriban
* LINGYUN.Abp.TextTemplating.Domain

### Features

* Provides Scriban template rendering engine
  * ScribanTemplateRenderingEngine - Scriban template rendering engine implementation
* Supports the following features
  * Write templates using Scriban syntax
  * Support model binding
  * Support layout templates
  * Support conditional statements and loops
  * Support custom functions and filters
  * Support string operations and formatting
  * Support array and object operations

### How to Use

1. Add `AbpTextTemplatingScribanModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingScribanModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Create and use Scriban templates

```csharp
// Create template definition
var template = new TextTemplateDefinition(
    Guid.NewGuid(),
    "Welcome",
    "Welcome Email Template",
    renderEngine: "Scriban");

// Template content example
<!DOCTYPE html>
<html>
<body>
    <h1>Welcome {{ user.name }}!</h1>
    <p>Thank you for joining us.</p>
    {{ if is_first_time }}
        <p>Here are some tips to get started...</p>
    {{ end }}
    <ul>
    {{ for item in items }}
        <li>{{ item.name }}: {{ item.description }}</li>
    {{ end }}
    </ul>
</body>
</html>

// Use template
public class YourService
{
    private readonly ITemplateRenderer _templateRenderer;

    public YourService(ITemplateRenderer templateRenderer)
    {
        _templateRenderer = templateRenderer;
    }

    public async Task<string> RenderWelcomeEmailAsync(
        string userName, 
        bool isFirstTime,
        List<Item> items)
    {
        var model = new Dictionary<string, object>
        {
            ["user"] = new { name = userName },
            ["is_first_time"] = isFirstTime,
            ["items"] = items
        };

        return await _templateRenderer.RenderAsync(
            "Welcome",
            model
        );
    }
}
```

[查看中文](README.md)
