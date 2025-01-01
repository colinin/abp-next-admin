# LINGYUN.Abp.Identity.Session.AspNetCore

Identity service user session extension module.

## Interface Description

### AbpSessionMiddleware extracts *sessionId* from user token in the request pipeline as a global session identifier, which can be used to log out sessions
> Note: When anonymous users access, the request *CorrelationId* is used as the identifier;
        When *CorrelationId* does not exist, use random *Guid.NewGuid()*.

### HttpContextDeviceInfoProvider extracts device identification from request parameters

> Due to module responsibility separation principle, please do not confuse with *LINGYUN.Abp.Identity.AspNetCore.Session* module

### HttpContextDeviceInfoProvider is used to handle session IP address location parsing

> IsParseIpLocation will parse the geographical location of session IP when enabled
> IgnoreProvinces are provinces that need to be ignored when parsing geographical locations, usually China's municipalities
> LocationParser customizes the geographical location data that needs to be processed

## Features

* Provides session management functionality in AspNetCore environment
* Supports extracting device identification information from requests
* Supports IP geolocation parsing
* Depends on AbpAspNetCoreModule, AbpIP2RegionModule, and AbpIdentitySessionModule

## Configuration Options

### AbpIdentitySessionAspNetCoreOptions

```json
{
  "Identity": {
    "Session": {
      "AspNetCore": {
        "IsParseIpLocation": false,    // Whether to parse IP geographic information, default: false
        "IgnoreProvinces": [          // Provinces to ignore, default includes China's municipalities
          "Beijing",
          "Shanghai",
          "Tianjin",
          "Chongqing"
        ]
      }
    }
  }
}
```

## Basic Usage

1. Configure IP geolocation parsing
```csharp
Configure<AbpIdentitySessionAspNetCoreOptions>(options =>
{
    options.IsParseIpLocation = true;    // Enable IP geolocation parsing
    options.IgnoreProvinces.Add("Hong Kong"); // Add provinces to ignore
    options.LocationParser = (locationInfo) => 
    {
        // Custom geolocation parsing logic
        return $"{locationInfo.Country}{locationInfo.Province}{locationInfo.City}";
    };
});
```

2. Use device information provider
```csharp
public class YourService
{
    private readonly IDeviceInfoProvider _deviceInfoProvider;

    public YourService(IDeviceInfoProvider deviceInfoProvider)
    {
        _deviceInfoProvider = deviceInfoProvider;
    }

    public async Task<DeviceInfo> GetDeviceInfoAsync()
    {
        return await _deviceInfoProvider.GetDeviceInfoAsync();
    }
}
```

## Configuration Usage

```csharp
[DependsOn(typeof(AbpIdentitySessionAspNetCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [ABP AspNetCore Documentation](https://docs.abp.io/en/abp/latest/AspNetCore)
