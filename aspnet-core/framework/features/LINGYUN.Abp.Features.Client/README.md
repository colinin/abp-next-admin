# LINGYUN.Abp.Features.Client

针对客户端的功能验证

## 功能特性

* 提供客户端功能值提供者(ClientFeatureValueProvider)
* 支持基于客户端ID的功能验证
* 与ABP框架的功能管理系统无缝集成

## 配置使用

1. 添加模块依赖

```csharp
[DependsOn(typeof(AbpFeaturesClientModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 使用示例

```csharp
public class YourService
{
    private readonly IFeatureChecker _featureChecker;

    public YourService(IFeatureChecker featureChecker)
    {
        _featureChecker = featureChecker;
    }

    public async Task DoSomethingAsync()
    {
        // 检查客户端是否启用某个功能
        if (await _featureChecker.IsEnabledAsync("YourFeature"))
        {
            // 业务逻辑
        }
    }
}
```

## 更多

* 本模块主要用于客户端功能验证，通常与LINGYUN.Abp.FeatureManagement.Client模块配合使用
* 客户端功能值提供者的名称为"C"

[简体中文](./README.md) | [English](./README.EN.md)
