# LINGYUN.Abp.Identity.Session

User session foundation module, providing related common interfaces.

## Features

* Provides basic interfaces for user session management
* Provides session cache and persistence synchronization mechanism
* Supports session access time tracking
* Depends on AbpCachingModule

## Configuration Usage

```csharp
[DependsOn(typeof(AbpIdentitySessionModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<IdentitySessionCheckOptions>(options =>
        {
            // Set session cache and persistence refresh interval to 10 minutes
            options.KeepAccessTimeSpan = TimeSpan.FromMinutes(10);
        });
    }
}
```

## Configuration Options

### IdentitySessionCheckOptions

```json
{
  "Identity": {
    "Session": {
      "Check": {
        "KeepAccessTimeSpan": "00:01:00",    // Access retention duration (cache session refresh interval), default: 1 minute
        "SessionSyncTimeSpan": "00:10:00"    // Session sync interval (sync from cache to persistence), default: 10 minutes
      }
    }
  }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
