# LINGYUN.Abp.TextTemplating.Razor

## Module Description

Text templating Razor engine module, providing template rendering implementation based on Razor syntax.

### Base Modules

* Volo.Abp.TextTemplating.Razor
* LINGYUN.Abp.TextTemplating.Domain

### Features

* Provides Razor template rendering engine
  * RazorTemplateRenderingEngine - Razor template rendering engine implementation
* Supports the following features
  * Write templates using Razor syntax
  * Support model binding and strongly-typed views
  * Support layout templates
  * Support partial views
  * Support HTML encoding and decoding
  * Support conditional statements and loops
  * Support C# expressions

### How to Use

1. Add `AbpTextTemplatingRazorModule` dependency

```csharp
[DependsOn(typeof(AbpTextTemplatingRazorModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Create and use Razor templates

```csharp
// Create template definition
var template = new TextTemplateDefinition(
    Guid.NewGuid(),
    "Welcome",
    "Welcome Email Template",
    renderEngine: "Razor");

// Template content example
@model WelcomeEmailModel

<!DOCTYPE html>
<html>
<body>
    <h1>Welcome @Model.UserName!</h1>
    <p>Thank you for joining us.</p>
    @if (Model.IsFirstTime)
    {
        <p>Here are some tips to get started...</p>
    }
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

    public async Task<string> RenderWelcomeEmailAsync(string userName, bool isFirstTime)
    {
        var model = new WelcomeEmailModel
        {
            UserName = userName,
            IsFirstTime = isFirstTime
        };

        return await _templateRenderer.RenderAsync(
            "Welcome",
            model
        );
    }
}
```

[查看中文](README.md)
