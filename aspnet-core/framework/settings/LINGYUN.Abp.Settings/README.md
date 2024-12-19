# LINGYUN.Abp.Settings

## 模块说明

ABP 设置管理扩展模块，提供了额外的设置管理功能。

### 基础模块  

* Volo.Abp.Settings

### 功能定义  

* 扩展了 ISettingProvider 接口，提供了更多便利的设置获取方法
  * GetOrDefaultAsync - 获取设置值，如果为空则返回默认值

### 配置定义  

无特殊配置项

### 如何使用

1. 添加模块依赖

```csharp
[DependsOn(typeof(AbpSettingsModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用扩展方法

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
        // 获取设置值，如果为空则返回默认值
        return await _settingProvider.GetOrDefaultAsync(name, _serviceProvider);
    }
}
```

[返回目录](../../../README.md)
