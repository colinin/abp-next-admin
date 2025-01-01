# LINGYUN.Abp.Localization.Persistence

## 模块说明

本地化组件持久层模块，提供将本地化资源持久化到存储设施的功能。此模块允许你将静态本地化文档保存到持久化存储中，方便管理和维护。

## 功能特性

* 支持将静态本地化资源持久化到存储设施
* 提供本地化资源的读写接口
* 支持自定义持久化存储实现
* 支持异步读写操作
* 支持多语言文化支持
* 支持选择性持久化指定的资源

## 安装

```bash
dotnet add package LINGYUN.Abp.Localization.Persistence
```

## 基础模块

* Volo.Abp.Localization

## 配置说明

模块提供以下配置选项：

* SaveStaticLocalizationsToPersistence：是否启用本地化资源持久化（默认：true）
* SaveToPersistenceResources：需要持久化的资源列表

## 使用方法

1. 添加模块依赖：

```csharp
[DependsOn(
    typeof(AbpLocalizationPersistenceModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationPersistenceOptions>(options =>
        {
            // 启用持久化设施
            options.SaveStaticLocalizationsToPersistence = true;

            // 指定你的本地化资源类型, 此类型下定义的静态文档将被持久化到存储设施
            options.AddPersistenceResource<YouProjectResource>();
        });

        // 或者使用扩展方法持久化本地化资源类型
        Configure<AbpLocalizationOptions>(options =>
        {
            // 效果如上
            options.UsePersistence<YouProjectResource>();
        });
    }
}
```

## 扩展接口

### ILocalizationPersistenceReader

用于从持久化存储中读取本地化资源：

```csharp
public interface ILocalizationPersistenceReader
{
    // 获取指定资源的本地化字符串
    LocalizedString GetOrNull(string resourceName, string cultureName, string name);

    // 填充本地化字典
    void Fill(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary);

    // 异步填充本地化字典
    Task FillAsync(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary);

    // 获取支持的文化列表
    Task<IEnumerable<string>> GetSupportedCulturesAsync();
}
```

### ILocalizationPersistenceWriter

用于将本地化资源写入持久化存储：

```csharp
public interface ILocalizationPersistenceWriter
{
    // 写入语言信息
    Task<bool> WriteLanguageAsync(LanguageInfo language);

    // 写入资源信息
    Task<bool> WriteResourceAsync(LocalizationResourceBase resource);

    // 获取已存在的文本
    Task<IEnumerable<string>> GetExistsTextsAsync(
        string resourceName,
        string cultureName,
        IEnumerable<string> keys);

    // 写入本地化文本
    Task<bool> WriteTextsAsync(IEnumerable<LocalizableStringText> texts);
}
```

## 自定义持久化实现

要实现自定义的持久化存储，需要：

1. 实现 `ILocalizationPersistenceReader` 接口
2. 实现 `ILocalizationPersistenceWriter` 接口
3. 在模块中注册你的实现：

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<ILocalizationPersistenceReader, YourCustomReader>();
    context.Services.AddTransient<ILocalizationPersistenceWriter, YourCustomWriter>();
}
```

## 更多信息

* [English Documentation](./README.EN.md)
* [ABP本地化文档](https://docs.abp.io/zh-Hans/abp/latest/Localization)
