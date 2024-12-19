# LINGYUN.Abp.BackgroundWorkers.Hangfire

English | [简体中文](README.md)

## 1. Introduction

`LINGYUN.Abp.BackgroundWorkers.Hangfire` is an ABP background worker module implemented based on Hangfire. It provides a reliable way to manage and execute long-running background tasks, supporting automatic start, stop, and periodic execution features.

## 2. Features

* Support for automatic start and stop of background workers
* Support for periodically executing background tasks
* Seamless integration with ABP background worker system
* Worker status management
* Dependency injection support

## 3. Installation

```bash
dotnet add package LINGYUN.Abp.BackgroundWorkers.Hangfire
```

## 4. Usage

1. Add `AbpBackgroundWorkersHangfireModule` to your module dependencies:

```csharp
[DependsOn(typeof(AbpBackgroundWorkersHangfireModule))]
public class YourModule : AbpModule
{
}
```

2. Create background worker:

```csharp
public class YourBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    public YourBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory) 
        : base(timer, serviceScopeFactory)
    {
        Timer.Period = 5000; // Execute every 5 seconds
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        // Implement your background task logic here
        await Task.CompletedTask;
    }
}
```

3. Register background worker:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<YourBackgroundWorker>();
    Configure<AbpBackgroundWorkerOptions>(options =>
    {
        options.IsEnabled = true; // Enable background workers
    });
}
```

## 5. Configuration

### 5.1 Worker Configuration

* `AbpBackgroundWorkerOptions.IsEnabled`: Whether to enable background workers. Default value: `true`

## 6. Dependencies

* Volo.Abp.BackgroundWorkers
* LINGYUN.Abp.BackgroundJobs.Hangfire

## 7. Documentation and Resources

* [Hangfire Official Documentation](https://docs.hangfire.io/)
* [ABP Framework Documentation](https://docs.abp.io/)
