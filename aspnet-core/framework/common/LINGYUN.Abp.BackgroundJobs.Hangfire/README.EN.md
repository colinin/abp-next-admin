# LINGYUN.Abp.BackgroundJobs.Hangfire

English | [简体中文](README.md)

## 1. Introduction

`LINGYUN.Abp.BackgroundJobs.Hangfire` is an ABP background job module implemented based on [Hangfire](https://www.hangfire.io/). It provides a reliable background job execution framework that supports immediate, delayed, and recurring task execution.

## 2. Features

* Support for immediate task execution
* Support for delayed task execution
* Support for recurring tasks (using Cron expressions)
* Seamless integration with ABP background job system
* Job execution status tracking
* Job retry mechanism

## 3. Installation

```bash
dotnet add package LINGYUN.Abp.BackgroundJobs.Hangfire
```

## 4. Usage

1. Add `AbpBackgroundJobsHangfireModule` to your module dependencies:

```csharp
[DependsOn(typeof(AbpBackgroundJobsHangfireModule))]
public class YourModule : AbpModule
{
}
```

2. Configure background jobs:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpBackgroundJobOptions>(options =>
    {
        options.IsJobExecutionEnabled = true; // Enable job execution
    });
}
```

3. Use background jobs:

```csharp
public class YourService
{
    private readonly IBackgroundJobManager _backgroundJobManager;

    public YourService(IBackgroundJobManager backgroundJobManager)
    {
        _backgroundJobManager = backgroundJobManager;
    }

    public async Task CreateJobAsync()
    {
        // Create immediate job
        await _backgroundJobManager.EnqueueAsync(new YourArgs());

        // Create delayed job
        await _backgroundJobManager.EnqueueAsync(
            new YourArgs(),
            delay: TimeSpan.FromMinutes(5)
        );

        // Create recurring job
        await _backgroundJobManager.EnqueueAsync(
            "0 0 * * *", // Cron expression: Execute at 00:00 every day
            new YourArgs()
        );
    }
}
```

## 5. Configuration

### 5.1 Job Configuration

* `AbpBackgroundJobOptions.IsJobExecutionEnabled`: Whether to enable job execution. Default value: `true`

## 6. Dependencies

* Volo.Abp.BackgroundJobs
* Hangfire.Core
* Hangfire.AspNetCore

## 7. Documentation and Resources

* [Hangfire Official Documentation](https://docs.hangfire.io/)
* [ABP Framework Documentation](https://docs.abp.io/)
