# LINGYUN.Abp.DistributedLocking.Dapr

Abp分布式锁的Dapr实现  

See: https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/distributed-lock-api-overview/

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDistributedLockingDaprModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedLockingDaprOptions>(options =>
        {
            options.StoreName = "store-name";
            options.ResourceId = "resource-id";
            options.DefaultTimeout = TimeSpan.FromSeconds(30);
        });
    }
}
```

## 配置说明

* AbpDistributedLockingDaprOptions.StoreName        在dapr component文件中定义的metadata name,默认: lockstore;
* AbpDistributedLockingDaprOptions.ResourceId       自定义一个资源名称,用于向dapr提供锁定的标识,默认: dapr-lock-id;
* AbpDistributedLockingDaprOptions.DefaultTimeout    默认锁定超时时间,默认: 30s.


## 其他
