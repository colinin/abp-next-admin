# LINGYUN.Abp.TextTemplating.Application

## 模块说明

文本模板应用服务模块，实现文本模板的管理和操作功能。

### 基础模块

* LINGYUN.Abp.TextTemplating.Application.Contracts
* Volo.Abp.TextTemplating
* Volo.Abp.Ddd.Application

### 功能定义

* 提供文本模板定义管理服务
  * TextTemplateDefinitionAppService - 模板定义管理服务
  * TextTemplateContentAppService - 模板内容管理服务
* 实现以下应用服务接口
  * ITextTemplateDefinitionAppService
  * ITextTemplateContentAppService

### 应用服务

* TextTemplateDefinitionAppService
  * GetAsync - 获取模板定义
  * GetListAsync - 获取模板定义列表
  * CreateAsync - 创建模板定义
  * UpdateAsync - 更新模板定义
  * DeleteAsync - 删除模板定义
* TextTemplateContentAppService
  * GetAsync - 获取模板内容
  * UpdateAsync - 更新模板内容
  * DeleteAsync - 删除模板内容
  * RestoreAsync - 恢复模板内容

### 权限

* AbpTextTemplating.TextTemplateDefinitions
  * Create - 创建模板定义
  * Update - 更新模板定义
  * Delete - 删除模板定义
* AbpTextTemplating.TextTemplateContents
  * Update - 更新模板内容
  * Delete - 删除模板内容

### 如何使用

1. 添加 `AbpTextTemplatingApplicationModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingApplicationModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 注入并使用模板服务

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
        // 创建模板定义
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

[查看英文](README.EN.md)
