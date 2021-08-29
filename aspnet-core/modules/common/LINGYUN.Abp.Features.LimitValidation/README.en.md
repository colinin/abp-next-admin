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

public static class FakeFeatureNames 
{
    public const string GroupName = "FakeFeature.Tests";
    // Type Limit feature name
    public const string ClassLimitFeature = GroupName + ".LimitFeature";
    // Method limits the number of calls to the feature name
    public const string MethodLimitFeature = GroupName + ".MethodLimitFeature";
    // Limit the call interval feature name
    public const string IntervalFeature = GroupName + ".IntervalFeature";
}

// Traffic limiting depends on user-defined features
public class FakeFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var featureGroup = context.AddGroup(FakeFeatureNames.GroupName);
        featureGroup.AddFeature(
            name: FakeFeatureNames.ClassLimitFeature,
            defaultValue: 1000.ToString(), // A maximum of 1000 calls can be made within a period
            valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
        featureGroup.AddFeature(
            name: FakeFeatureNames.MethodLimitFeature,
            defaultValue: 100.ToString(), // A maximum of 100 calls can be made within a period
            valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
        featureGroup.AddFeature(
            name: FakeFeatureNames.IntervalFeature,
            defaultValue: 1.ToString(),   // Limit cycle
            valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
    }
}

// By default, the type is allowed to be called only 1000 times a day
[RequiresLimitFeature(FakeFeatureNames.ClassLimitFeature, FakeFeatureNames.IntervalFeature, LimitPolicy.Days)]
public class FakeLimitClass
{
    // By default, the method is only allowed to be called 100 times a minute
    [RequiresLimitFeature(FakeFeatureNames.MethodLimitFeature, FakeFeatureNames.IntervalFeature, LimitPolicy.Minute)]
    public void LimitMethod() 
    {
        // other...
    }
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
