# LINGYUN.Abp.Settings

## Module Description

ABP Settings Management extension module, providing additional settings management functionality.

### Base Modules  

* Volo.Abp.Settings

### Features  

* Extends the ISettingProvider interface, providing more convenient setting retrieval methods
  * GetOrDefaultAsync - Get setting value, returns default value if empty

### Configuration  

No special configuration items

### How to Use

1. Add module dependency

```csharp
[DependsOn(typeof(AbpSettingsModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use extension methods

```csharp
public class YourService
{
    private readonly ISettingProvider _settingProvider;
    private readonly IServiceProvider _serviceProvider;

    public YourService(
        ISettingProvider settingProvider,
        IServiceProvider serviceProvider)
    {
        _settingProvider = settingProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GetSettingValueAsync(string name)
    {
        // Get setting value, returns default value if empty
        return await _settingProvider.GetOrDefaultAsync(name, _serviceProvider);
    }
}
```

[Back to TOC](../../../README.md)
