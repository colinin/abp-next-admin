# LINGYUN.Abp.TextTemplating.Razor

## 模块说明

文本模板 Razor 引擎模块，提供基于 Razor 语法的模板渲染实现。

### 基础模块

* Volo.Abp.TextTemplating.Razor
* LINGYUN.Abp.TextTemplating.Domain

### 功能定义

* 提供 Razor 模板渲染引擎
  * RazorTemplateRenderingEngine - Razor 模板渲染引擎实现
* 支持以下功能
  * 使用 Razor 语法编写模板
  * 支持模型绑定和强类型视图
  * 支持布局模板
  * 支持部分视图
  * 支持 HTML 编码和解码
  * 支持条件语句和循环
  * 支持 C# 表达式

### 如何使用

1. 添加 `AbpTextTemplatingRazorModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingRazorModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 创建和使用 Razor 模板

```csharp
// 创建模板定义
var template = new TextTemplateDefinition(
    Guid.NewGuid(),
    "Welcome",
    "Welcome Email Template",
    renderEngine: "Razor");

// 模板内容示例
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

// 使用模板
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

[查看英文](README.EN.md)
