English | [简体中文](./README.md)

# LINGYUN.Abp.Features.LimitValidation

Features limit validation component

Check the number of function calls defined to limit calls to the application by specific entities (tenants, users, clients, and so on)

Predefined policy

LimitPolicy.Minute		Calculate the flow by the minutes
LimitPolicy.Hours		Calculate the flow by the hours
LimitPolicy.Days		Calculate the flow by days
LimitPolicy.Weeks		Calculate the flow by weeks
LimitPolicy.Month		Calculate the flow by the number of month
LimitPolicy.Years		Calculate the flow by the number of years

## How to use


```csharp
[DependsOn(typeof(AbpFeaturesLimitValidationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

If you want to limit the policy duration by self-processing, override the default policy for the corresponding policy and always return a clock scale in seconds

```csharp
[DependsOn(typeof(AbpFeaturesLimitValidationModule))]
public class YouProjectModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpFeaturesLimitValidationOptions>(options =>
        {
            options.MapEffectPolicy(LimitPolicy.Minute, (time) => return 60;); // Means that no matter how many minutes (time), only 60 seconds will be limited
         });
    }
}
```
