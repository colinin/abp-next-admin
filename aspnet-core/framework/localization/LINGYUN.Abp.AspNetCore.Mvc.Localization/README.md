# LINGYUN.Abp.AspNetCore.Mvc.Localization

ABP框架本地化管理模块，提供本地化资源的Web API接口。

## 功能特性

* 提供本地化文本管理API接口
* 支持多语言资源的查询和比较
* 支持按资源名称和文化名称过滤
* 支持外部本地化资源的集成
* 支持本地化资源的差异对比

## 安装

```bash
dotnet add package LINGYUN.Abp.AspNetCore.Mvc.Localization
```

## 模块依赖

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcLocalizationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API接口

### 语言管理

* GET /api/localization/languages - 获取所有可用语言
* GET /api/localization/languages/{cultureName} - 获取指定语言信息

### 资源管理

* GET /api/localization/resources - 获取所有本地化资源
* GET /api/localization/resources/{name} - 获取指定本地化资源

### 文本管理

* GET /api/localization/texts - 获取本地化文本列表
* GET /api/localization/texts/by-key - 根据键获取本地化文本
* GET /api/localization/texts/differences - 获取不同语言间的文本差异

## 基本用法

1. 配置本地化资源
```csharp
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Get<YourLocalizationResource>()
        .AddVirtualJson("/Your/Resource/Path");
});
```

2. 注入并使用服务
```csharp
public class YourService
{
    private readonly ITextAppService _textAppService;
    
    public YourService(ITextAppService textAppService)
    {
        _textAppService = textAppService;
    }
    
    public async Task GetLocalizedText(string key, string cultureName)
    {
        var text = await _textAppService.GetByCultureKeyAsync(new GetTextByKeyInput
        {
            Key = key,
            CultureName = cultureName,
            ResourceName = "YourResourceName"
        });
        // Use the localized text
    }
}
```

## 更多信息

* [English Documentation](./README.EN.md)
* [ABP本地化文档](https://docs.abp.io/zh-Hans/abp/latest/Localization)
