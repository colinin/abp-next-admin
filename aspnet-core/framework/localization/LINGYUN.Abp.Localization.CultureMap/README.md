# LINGYUN.Abp.Localization.CultureMap

## 模块说明

本模块用于解决存在多种格式的区域性本地化问题。它允许你将不同格式的区域性标识映射到标准格式。

参考项目: [Owl.Abp.CultureMap](https://github.com/maliming/Owl.Abp.CultureMap)

## 功能特性

* 支持将多种格式的区域性标识映射到标准格式
* 支持区域性（Culture）和UI区域性（UICulture）的独立映射
* 与ABP请求本地化集成
* 支持自定义区域性映射规则

## 安装

```bash
dotnet add package LINGYUN.Abp.Localization.CultureMap
```

## 基础模块

* Volo.Abp.AspNetCore

## 配置说明

模块提供了以下配置选项：

* CulturesMaps：区域性映射列表
* UiCulturesMaps：UI区域性映射列表

每个映射项包含：
* TargetCulture：目标区域性标识
* SourceCultures：源区域性标识列表

## 使用方法

1. 添加模块依赖：

```csharp
[DependsOn(
    typeof(AbpLocalizationCultureMapModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationCultureMapOptions>(options =>
        {
            var zhHansCultureMapInfo = new CultureMapInfo
            {
                TargetCulture = "zh-Hans",
                SourceCultures = new string[] { "zh", "zh_CN", "zh-CN" }
            };

            options.CulturesMaps.Add(zhHansCultureMapInfo);
            options.UiCulturesMaps.Add(zhHansCultureMapInfo);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseMapRequestLocalization();
    }
}
```

## 常见用例

1. 统一简体中文区域性标识：
```csharp
options.CulturesMaps.Add(new CultureMapInfo
{
    TargetCulture = "zh-Hans",
    SourceCultures = new string[] { "zh", "zh_CN", "zh-CN" }
});
```

2. 统一繁体中文区域性标识：
```csharp
options.CulturesMaps.Add(new CultureMapInfo
{
    TargetCulture = "zh-Hant",
    SourceCultures = new string[] { "zh_TW", "zh-TW", "zh_HK", "zh-HK" }
});
```

## 更多信息

* [English Documentation](./README.EN.md)
* [ABP本地化文档](https://docs.abp.io/zh-Hans/abp/latest/Localization)

## 更新日志 
