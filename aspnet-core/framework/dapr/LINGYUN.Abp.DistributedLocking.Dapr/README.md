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
            options.DefaultIdentifier = "default-owner-id";
            options.DefaultTimeout = TimeSpan.FromSeconds(30);
        });
    }
}
```

## 配置说明

* AbpDistributedLockingDaprOptions.StoreName            在dapr component文件中定义的metadata name,默认: lockstore;
* AbpDistributedLockingDaprOptions.DefaultIdentifier    默认锁资源拥有者标识,默认: dapr-lock-owner;
* AbpDistributedLockingDaprOptions.DefaultTimeout       默认锁定超时时间,默认: 30s.

## 接口说明

[ILockOwnerFinder](./LINGYUN/Abp/DistributedLocking/Dapr/ILockOwnerFinder), 提供锁资源持有者标识  
默认实现 [LockOwnerFinder](./LINGYUN/Abp/DistributedLocking/Dapr/LockOwnerFinder), 获取用户标识,如果不存在,返回DefaultIdentifier  

## 其他
