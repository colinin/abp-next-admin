[English](./README.en.md) | 简体中文

# LINGYUN.Abp.Features.LimitValidation

功能上限验证组件  

检查定义的功能调用次数,来限制特定的实体(租户、用户、客户端等)对于应用程序的调用  

预先设定了如下几个策略  

LimitPolicy.Minute		按分钟计算流量  
LimitPolicy.Hours		按小时计算流量  
LimitPolicy.Days		按天数计算流量  
LimitPolicy.Weeks		按周数计算流量  
LimitPolicy.Month		按月数计算流量  
LimitPolicy.Years		按年数计算流量  

## 配置使用


```csharp
[DependsOn(typeof(AbpFeaturesLimitValidationModule))]
public class YouProjectModule : AbpModule
{
  // other
}

public static class FakeFeatureNames 
{
    public const string GroupName = "FakeFeature.Tests";
    // 类型限制调用次数功能名称
    public const string ClassLimitFeature = GroupName + ".LimitFeature";
    // 方法限制调用次数功能名称
    public const string MethodLimitFeature = GroupName + ".MethodLimitFeature";
    // 限制调用间隔功能名称
    public const string IntervalFeature = GroupName + ".IntervalFeature";
}

// 流量限制依赖自定义功能
public class FakeFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var featureGroup = context.AddGroup(FakeFeatureNames.GroupName);
        featureGroup.AddFeature(
            name: FakeFeatureNames.ClassLimitFeature,
            defaultValue: 1000.ToString(), // 周期内最大允许调用1000次
            valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
        featureGroup.AddFeature(
            name: FakeFeatureNames.MethodLimitFeature,
            defaultValue: 100.ToString(), // 周期内最大允许调用100次
            valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
        featureGroup.AddFeature(
            name: FakeFeatureNames.IntervalFeature,
            defaultValue: 1.ToString(),   // 限制周期
            valueType: new ToggleStringValueType(new NumericValueValidator(1, 1000)));
    }
}

// 按照预设的参数,类型在一天钟内仅允许调用1000次
[RequiresLimitFeature(FakeFeatureNames.ClassLimitFeature, FakeFeatureNames.IntervalFeature, LimitPolicy.Days)]
public class FakeLimitClass
{
    // 按照预设的参数,方法在一分钟内仅允许调用100次
    [RequiresLimitFeature(FakeFeatureNames.MethodLimitFeature, FakeFeatureNames.IntervalFeature, LimitPolicy.Minute)]
    public void LimitMethod() 
    {
        // other...
    }
}
```

如果需要自行处理功能限制策略时长,请覆盖对应策略的默认策略,返回的时钟刻度单位始终是秒  

```csharp
[DependsOn(typeof(AbpFeaturesLimitValidationModule))]
public class YouProjectModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpFeaturesLimitValidationOptions>(options =>
        {
            options.MapEffectPolicy(LimitPolicy.Minute, (time) => return 60;); // 表示不管多少分钟(time),都只会限制60秒
         });
    }
}
```
