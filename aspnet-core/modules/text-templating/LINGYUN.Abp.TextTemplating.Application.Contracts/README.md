# LINGYUN.Abp.TextTemplating.Application.Contracts

## 模块说明

文本模板应用服务契约模块，提供文本模板管理相关的接口定义和DTO。

### 基础模块

* Volo.Abp.TextTemplating
* Volo.Abp.Ddd.Application.Contracts

### 功能定义

* 提供文本模板管理的应用服务接口
  * ITextTemplateDefinitionAppService - 模板定义管理服务接口
  * ITextTemplateContentAppService - 模板内容管理服务接口
* 提供文本模板相关的DTO定义
  * TextTemplateDefinitionDto - 模板定义DTO
  * TextTemplateContentDto - 模板内容DTO
  * TextTemplateDefinitionCreateDto - 创建模板定义DTO
  * TextTemplateDefinitionUpdateDto - 更新模板定义DTO
  * TextTemplateContentUpdateDto - 更新模板内容DTO
* 提供文本模板管理相关的权限定义

### 权限定义

* AbpTextTemplating.TextTemplateDefinitions
  * Create - 创建模板定义
  * Update - 更新模板定义
  * Delete - 删除模板定义
* AbpTextTemplating.TextTemplateContents
  * Update - 更新模板内容
  * Delete - 删除模板内容

### 应用服务接口

* ITextTemplateDefinitionAppService
  * GetAsync - 获取模板定义
  * GetListAsync - 获取模板定义列表
  * CreateAsync - 创建模板定义
  * UpdateAsync - 更新模板定义
  * DeleteAsync - 删除模板定义
* ITextTemplateContentAppService
  * GetAsync - 获取模板内容
  * UpdateAsync - 更新模板内容
  * DeleteAsync - 删除模板内容
  * RestoreAsync - 恢复模板内容

### 如何使用

1. 添加 `AbpTextTemplatingApplicationContractsModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 注入并使用模板服务接口

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
        // 获取模板定义列表
        var templates = await _templateDefinitionAppService.GetListAsync(
            new TextTemplateDefinitionGetListInput());
    }
}
```

[查看英文](README.EN.md)
