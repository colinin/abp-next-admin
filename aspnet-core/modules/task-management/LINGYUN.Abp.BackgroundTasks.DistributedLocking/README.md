# LINGYUN.Abp.BackgroundTasks.DistributedLocking

后台任务（队列）模块分布式锁模块  

See: [Distributed-Locking](https://docs.abp.io/en/abp/latest/Distributed-Locking)

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpBackgroundTasksDistributedLockingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
