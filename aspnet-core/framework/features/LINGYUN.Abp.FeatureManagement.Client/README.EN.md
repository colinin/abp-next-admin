# LINGYUN.Abp.FeatureManagement.Client

Client feature management authorization component

## Features

* Provides ClientFeatureManagementProvider
* Supports permission management for client features
* Supports localization resource management
* Seamlessly integrates with ABP framework's feature management module

## Configuration and Usage

1. Add module dependency

```csharp
[DependsOn(typeof(AbpFeatureManagementClientModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. Permission Configuration

The module predefines the following permissions:
* FeatureManagement.ManageClientFeatures: Permission to manage client features

3. Usage Example

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
        // Set client feature value
        await _featureManager.SetForClientAsync(clientId, featureName, value);
    }
}
```

## More

* This module depends on the LINGYUN.Abp.Features.Client module
* Supports localization configuration through AbpFeatureManagementResource
* Provides permission control for client feature management

[简体中文](./README.md) | [English](./README.EN.md)
