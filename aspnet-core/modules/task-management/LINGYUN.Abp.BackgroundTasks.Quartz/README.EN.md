# LINGYUN.Abp.BackgroundTasks.Quartz

Background task module implementation based on Quartz, with added listener functionality to notify administrators of task status.

## Configuration and Usage

Module reference (refer to Volo.Abp.Quartz module for detailed configuration):

```csharp
[DependsOn(typeof(AbpBackgroundTasksQuartzModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

### Job Scheduling
- Support for various job types:
  - One-time jobs
  - Periodic jobs (with Cron expressions)
  - Persistent jobs
- Job priority management
- Start/end time scheduling
- Interval-based execution

### Job Management
- Job queuing and execution
- Job pausing and resuming
- Job triggering on demand
- Job removal and cleanup

### Job Execution
- Concurrent job execution support
- Job execution context management
- Job parameter passing
- Job result handling

### Job Monitoring
- Job execution event listening
- Job status tracking
- Error handling and logging
- Multi-tenant support

### Distributed Features
- Distributed job locking
- Node-specific job execution
- Lock timeout management

### Adapters
- `QuartzJobSimpleAdapter`: For simple job execution
- `QuartzJobConcurrentAdapter`: For concurrent job execution
- `QuartzJobSearchJobAdapter`: For runtime job discovery
