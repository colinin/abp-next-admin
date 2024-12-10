# LINGYUN.Abp.Features.Client

Client feature validation component

## Features

* Provides ClientFeatureValueProvider
* Supports feature validation based on client ID
* Seamlessly integrates with ABP framework's feature management system

## Configuration and Usage

1. Add module dependency

```csharp
[DependsOn(typeof(AbpFeaturesClientModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. Usage example

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
        // Check if a feature is enabled for the client
        if (await _featureChecker.IsEnabledAsync("YourFeature"))
        {
            // Business logic
        }
    }
}
```

## More

* This module is mainly used for client feature validation, typically used in conjunction with the LINGYUN.Abp.FeatureManagement.Client module
* The name of the client feature value provider is "C"

[简体中文](./README.md) | [English](./README.EN.md)
