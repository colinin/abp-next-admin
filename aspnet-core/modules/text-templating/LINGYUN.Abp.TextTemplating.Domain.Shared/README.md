# LINGYUN.Abp.TextTemplating.Domain.Shared

## 模块说明

文本模板领域共享模块，提供文本模板相关的常量、枚举、异常等共享定义。

### 基础模块

* Volo.Abp.TextTemplating
* Volo.Abp.Validation

### 功能定义

* 提供文本模板相关的常量定义
  * TextTemplateDefinitionConsts - 模板定义相关常量
  * TextTemplateContentConsts - 模板内容相关常量
* 提供文本模板相关的错误代码定义
  * AbpTextTemplatingErrorCodes - 错误代码常量
* 提供本地化资源定义
  * AbpTextTemplatingResource - 本地化资源

### 常量定义

* TextTemplateDefinitionConsts
  * MaxNameLength - 模板名称最大长度 (64)
  * MaxDisplayNameLength - 显示名称最大长度 (128)
  * MaxLayoutLength - 布局名称最大长度 (256)
  * MaxDefaultCultureNameLength - 默认文化名称最大长度 (10)
  * MaxLocalizationResourceNameLength - 本地化资源名称最大长度 (128)
  * MaxRenderEngineLength - 渲染引擎名称最大长度 (64)

### 错误代码

* AbpTextTemplatingErrorCodes
  * TextTemplateDefinition:NameAlreadyExists - 模板名称已存在
  * TextTemplateDefinition:NotFound - 模板定义不存在

### 如何使用

1. 添加 `AbpTextTemplatingDomainSharedModule` 依赖

```csharp
[DependsOn(typeof(AbpTextTemplatingDomainSharedModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用常量和错误代码

```csharp
public class YourService
{
    public void ValidateTemplateName(string name)
    {
        if (name.Length > TextTemplateDefinitionConsts.MaxNameLength)
        {
            throw new BusinessException(AbpTextTemplatingErrorCodes.TextTemplateDefinition.NameAlreadyExists);
        }
    }
}
```

[查看英文](README.EN.md)
