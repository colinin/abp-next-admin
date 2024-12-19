# LINGYUN.Abp.TextTemplating.Scriban

## 模块说明

文本模板 Scriban 引擎模块，提供基于 Scriban 语法的模板渲染实现。

### 基础模块

* Volo.Abp.TextTemplating.Scriban
* LINGYUN.Abp.TextTemplating.Domain

### 功能定义

* 提供 Scriban 模板渲染引擎
  * ScribanTemplateRenderingEngine - Scriban 模板渲染引擎实现
* 支持以下功能
  * 使用 Scriban 语法编写模板
  * 支持模型绑定
  * 支持布局模板
  * 支持条件语句和循环
  * 支持自定义函数和过滤器
  * 支持字符串操作和格式化
  * 支持数组和对象操作

### 如何使用

1. 添加 `AbpTextTemplatingScribanModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingScribanModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 创建和使用 Scriban 模板

```csharp
// 创建模板定义
var template = new TextTemplateDefinition(
    Guid.NewGuid(),
    "Welcome",
    "Welcome Email Template",
    renderEngine: "Scriban");

// 模板内容示例
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

// 使用模板
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

[查看英文](README.EN.md)
