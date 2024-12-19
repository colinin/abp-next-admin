# LINGYUN.Abp.BackgroundTasks.Hangfire

Background task module implementation based on Hangfire.

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksHangfireModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

- Integrates with Hangfire for background job processing
- Supports job execution events and monitoring
- Implements distributed locking for job execution
- Provides job execution context and result handling
- Includes job parameter management
- Supports job cancellation and timeout handling

## Components

- `HangfireJobExecutedAttribute`: Handles job execution events and locking
- `HangfireJobSimpleAdapter`: Adapts job execution to the Hangfire infrastructure
- Job event handling with support for:
  - Before execution events
  - After execution events
  - Execution result handling
  - Error handling and logging

## Job Execution Flow

1. Job scheduling through Hangfire
2. Pre-execution locking (if configured)
3. Job execution with context
4. Event handling and result processing
5. Lock release and cleanup
