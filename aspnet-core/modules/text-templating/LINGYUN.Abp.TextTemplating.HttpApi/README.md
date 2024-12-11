# LINGYUN.Abp.TextTemplating.HttpApi

## 模块说明

文本模板 HTTP API 模块，提供文本模板管理的 RESTful API 接口。

### 基础模块

* LINGYUN.Abp.TextTemplating.Application.Contracts
* Volo.Abp.AspNetCore.Mvc

### 功能定义

* 提供文本模板管理的 API 控制器
  * TextTemplateDefinitionController - 模板定义管理控制器
  * TextTemplateContentController - 模板内容管理控制器

### API 接口

* /api/text-templating/template-definitions
  * GET - 获取模板定义列表
  * POST - 创建模板定义
  * PUT - 更新模板定义
  * DELETE - 删除模板定义
  * GET /{name} - 获取指定模板定义
* /api/text-templating/template-contents
  * GET - 获取模板内容
  * PUT - 更新模板内容
  * DELETE - 删除模板内容
  * POST /restore - 恢复模板内容

### 权限要求

* AbpTextTemplating.TextTemplateDefinitions
  * Create - 创建模板定义
  * Update - 更新模板定义
  * Delete - 删除模板定义
* AbpTextTemplating.TextTemplateContents
  * Update - 更新模板内容
  * Delete - 删除模板内容

### 如何使用

1. 添加 `AbpTextTemplatingHttpApiModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingHttpApiModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用 API 接口

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
        // 获取模板定义列表
        var response = await _httpClient.GetAsync("/api/text-templating/template-definitions");
        var templates = await response.Content.ReadFromJsonAsync<ListResultDto<TextTemplateDefinitionDto>>();
    }
}
```

[查看英文](README.EN.md)
