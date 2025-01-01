# LINGYUN.Abp.Localization.Persistence

## Module Description

Localization component persistence module, providing functionality to persist localization resources to storage facilities. This module allows you to save static localization documents to persistent storage for easier management and maintenance.

## Features

* Support persisting static localization resources to storage facilities
* Provide read and write interfaces for localization resources
* Support custom persistence storage implementation
* Support asynchronous read and write operations
* Support multi-language culture support
* Support selective persistence of specified resources

## Installation

```bash
dotnet add package LINGYUN.Abp.Localization.Persistence
```

## Base Modules

* Volo.Abp.Localization

## Configuration

The module provides the following configuration options:

* SaveStaticLocalizationsToPersistence: Whether to enable localization resource persistence (default: true)
* SaveToPersistenceResources: List of resources to be persisted

## Usage

1. Add module dependency:

```csharp
[DependsOn(
    typeof(AbpLocalizationPersistenceModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationPersistenceOptions>(options =>
        {
            // Enable persistence facility
            options.SaveStaticLocalizationsToPersistence = true;

            // Specify your localization resource type, static documents under this type will be persisted to storage facilities
            options.AddPersistenceResource<YouProjectResource>();
        });

        // Or use extension method to persist localization resource type
        Configure<AbpLocalizationOptions>(options =>
        {
            // Same effect as above
            options.UsePersistence<YouProjectResource>();
        });
    }
}
```

## Extension Interfaces

### ILocalizationPersistenceReader

Used to read localization resources from persistent storage:

```csharp
public interface ILocalizationPersistenceReader
{
    // Get localized string for specified resource
    LocalizedString GetOrNull(string resourceName, string cultureName, string name);

    // Fill localization dictionary
    void Fill(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary);

    // Asynchronously fill localization dictionary
    Task FillAsync(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary);

    // Get supported cultures list
    Task<IEnumerable<string>> GetSupportedCulturesAsync();
}
```

### ILocalizationPersistenceWriter

Used to write localization resources to persistent storage:

```csharp
public interface ILocalizationPersistenceWriter
{
    // Write language information
    Task<bool> WriteLanguageAsync(LanguageInfo language);

    // Write resource information
    Task<bool> WriteResourceAsync(LocalizationResourceBase resource);

    // Get existing texts
    Task<IEnumerable<string>> GetExistsTextsAsync(
        string resourceName,
        string cultureName,
        IEnumerable<string> keys);

    // Write localization texts
    Task<bool> WriteTextsAsync(IEnumerable<LocalizableStringText> texts);
}
```

## Custom Persistence Implementation

To implement custom persistence storage, you need to:

1. Implement `ILocalizationPersistenceReader` interface
2. Implement `ILocalizationPersistenceWriter` interface
3. Register your implementation in the module:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<ILocalizationPersistenceReader, YourCustomReader>();
    context.Services.AddTransient<ILocalizationPersistenceWriter, YourCustomWriter>();
}
```

## More Information

* [中文文档](./README.md)
* [ABP Localization Documentation](https://docs.abp.io/en/abp/latest/Localization)
