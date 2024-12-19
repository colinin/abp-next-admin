# LINGYUN.Abp.BackgroundTasks.EventBus

Background task (queue) module distributed event module, integrating this module enables applications to handle job events.

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

The module provides various job event types:

- JobEventData - Base event data for all job events
- JobStartEventData - Event data when a job starts
- JobStopEventData - Event data when a job stops
- JobPauseEventData - Event data when a job is paused
- JobResumeEventData - Event data when a job resumes
- JobTriggerEventData - Event data when a job is triggered
