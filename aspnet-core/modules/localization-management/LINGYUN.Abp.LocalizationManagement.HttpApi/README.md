# LINGYUN.Abp.LocalizationManagement.HttpApi

本地化管理HTTP API模块，提供RESTful风格的API接口。

## 功能特性

* 提供本地化管理的HTTP API接口
* 支持语言、资源、文本的REST操作
* 集成ABP动态API功能
* 支持API版本控制
* 支持Swagger文档生成

## 模块引用

```csharp
[DependsOn(typeof(AbpLocalizationManagementHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API控制器

* `LanguageController`: 语言管理API控制器
* `ResourceController`: 资源管理API控制器
* `TextController`: 文本管理API控制器

## 配置项

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
    {
        options.AddAssemblyResource(
            typeof(LocalizationManagementResource),
            typeof(AbpLocalizationManagementApplicationContractsModule).Assembly);
    });

    PreConfigure<IMvcBuilder>(mvcBuilder =>
    {
        mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpLocalizationManagementApplicationContractsModule).Assembly);
    });
}
```

## API路由

* `/api/localization-management/languages` - 语言管理相关API
* `/api/localization-management/resources` - 资源管理相关API
* `/api/localization-management/texts` - 文本管理相关API

## 更多信息

* [English documentation](./README.EN.md)
