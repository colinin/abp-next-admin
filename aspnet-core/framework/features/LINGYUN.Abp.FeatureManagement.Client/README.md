# LINGYUN.Abp.FeatureManagement.Client

针对客户端的功能验证管理授权

## 功能特性

* 提供客户端功能管理提供者(ClientFeatureManagementProvider)
* 支持客户端功能的权限管理
* 支持本地化资源管理
* 与ABP框架的功能管理模块无缝集成

## 配置使用

1. 添加模块依赖

```csharp
[DependsOn(typeof(AbpFeatureManagementClientModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 权限配置

模块预定义了以下权限：
* FeatureManagement.ManageClientFeatures：管理客户端功能的权限

3. 使用示例

```csharp
public class YourService
{
    private readonly IFeatureManager _featureManager;

    public YourService(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async Task SetClientFeatureAsync(string clientId, string featureName, string value)
    {
        // 设置客户端功能值
        await _featureManager.SetForClientAsync(clientId, featureName, value);
    }
}
```

## 更多

* 本模块依赖于LINGYUN.Abp.Features.Client模块
* 支持通过AbpFeatureManagementResource进行本地化配置
* 提供了针对客户端功能管理的权限控制

[简体中文](./README.md) | [English](./README.EN.md)
