# LINGYUN.Abp.BackgroundTasks.DistributedLocking

Background task (queue) module distributed locking module.

See: [Distributed-Locking](https://docs.abp.io/en/abp/latest/Distributed-Locking)

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksDistributedLockingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
