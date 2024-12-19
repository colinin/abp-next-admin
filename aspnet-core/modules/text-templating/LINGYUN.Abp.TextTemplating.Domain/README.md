# LINGYUN.Abp.TextTemplating.Domain

## 模块说明

文本模板领域模块，提供文本模板定义和内容管理的核心功能。

### 基础模块

* Volo.Abp.TextTemplating
* Volo.Abp.Ddd.Domain

### 功能定义

* 提供文本模板定义的领域实体
  * TextTemplateDefinition - 文本模板定义实体
  * TextTemplateContent - 文本模板内容实体
* 提供文本模板定义的仓储接口
  * ITextTemplateDefinitionRepository - 文本模板定义仓储接口
  * ITextTemplateContentRepository - 文本模板内容仓储接口
* 提供文本模板管理的领域服务
  * TextTemplateManager - 文本模板管理器
  * IStaticTemplateDefinitionStore - 静态模板定义存储
  * IDynamicTemplateDefinitionStore - 动态模板定义存储

### 领域服务

* TextTemplateManager
  * 管理文本模板的创建、更新、删除
  * 处理模板定义与内容的关联
  * 支持静态和动态模板定义的管理

### 实体属性

* TextTemplateDefinition
  * Name - 模板名称
  * DisplayName - 显示名称
  * IsLayout - 是否为布局模板
  * Layout - 布局名称
  * IsInlineLocalized - 是否内联本地化
  * DefaultCultureName - 默认文化名称
  * LocalizationResourceName - 本地化资源名称
  * RenderEngine - 渲染引擎
  * IsStatic - 是否为静态模板

### 如何使用

1. 添加 `AbpTextTemplatingDomainModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingDomainModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用文本模板管理器

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
        // 创建模板定义
        var template = new TextTemplateDefinition(
            Guid.NewGuid(),
            "TemplateName",
            "Template Display Name",
            renderEngine: "Razor");

        await _templateManager.CreateAsync(template);
    }
}
```

[查看英文](README.EN.md)
