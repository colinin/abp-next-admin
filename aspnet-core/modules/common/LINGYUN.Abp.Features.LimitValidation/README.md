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
