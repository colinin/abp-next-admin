# LINGYUN.Abp.BackgroundTasks.Activities

Background task (queue) module behavior processing module.

## Interface Parameters

* IJobActionStore: Implement this interface to get job management behaviors
* JobActionDefinitionProvider: Implement this interface to customize job behaviors
* JobExecutedProvider: Implement this interface to extend job behaviors

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksActivitiesModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
