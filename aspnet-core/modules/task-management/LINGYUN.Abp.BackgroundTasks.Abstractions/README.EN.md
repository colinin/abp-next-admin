# LINGYUN.Abp.BackgroundTasks.Abstractions

Background task (queue) module abstraction layer, defining basic constructs and interfaces.

## Feature Parameters

* DisableJobActionAttribute: Mark this feature to disable job trigger behavior processing
* DisableJobStatusAttribute: Mark this feature to disable job status processing
* DisableAuditingAttribute: Mark this feature to disable job logging

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
