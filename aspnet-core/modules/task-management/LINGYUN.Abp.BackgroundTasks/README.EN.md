# LINGYUN.Abp.BackgroundTasks

Background task (queue) module that extends ABP's background jobs and workers with Cron expression support, providing manageable background task (queue) functionality.

Implements **Volo.Abp.BackgroundJobs.IBackgroundJobManager**, meaning you can add new jobs through the framework's background job interface.  
Implements **Volo.Abp.BackgroundWorkers.IBackgroundWorkerManager**, meaning you can add new jobs through the framework's background worker interface.  

## Task Types

* JobType.Once:         One-time task, executed only once, suitable for scenarios like email notifications
* JobType.Period:       Periodic task, runs according to a Cron expression, suitable for scenarios like report analysis
* JobType.Persistent:   Persistent task, runs according to given repeat count and interval, suitable for scenarios like API stress testing

## Interface Description

* [IJobPublisher](/LINGYUN/Abp/BackgroundTasks/IJobPublisher.cs): Job publishing interface, publishes specified jobs to the current node
* [IJobDispatcher](/LINGYUN/Abp/BackgroundTasks/IJobDispatcher.cs): Job dispatching interface, dispatches specified jobs to specified nodes
* [IJobScheduler](/LINGYUN/Abp/BackgroundTasks/IJobScheduler.cs): Scheduler interface, manages job scheduler for the current running node
* [IJobLockProvider](/LINGYUN/Abp/BackgroundTasks/IJobLockProvider.cs): Job locking interface, locks specified jobs to prevent duplicate execution, lock duration is specified by **LockTimeOut**
* [IJobEventTrigger](/LINGYUN/Abp/BackgroundTasks/IJobEventTrigger.cs): Job event trigger interface, listens to events before and after job execution
* [IJobStore](/LINGYUN/Abp/BackgroundTasks/IJobStore.cs): Job persistence interface

## Configuration and Usage

Module reference:

```csharp
[DependsOn(typeof(AbpBackgroundTasksModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

Example usage:

```csharp
public class DemoClass
{
    protected IServiceProvider ServiceProvider { get; }

    public DemoClass(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async Task Some()
    {
        var scheduler = ServiceProvider.GetRequiredService<IJobScheduler>();

        // Add a periodic task (every 5 seconds) to the queue
        await scheduler.QueueAsync(new JobInfo
        {
            Type = typeof(ConsoleJob).AssemblyQualifiedName,
            Args = new Dictionary<string, object>(),
            Name = "Test-Console-Period",
            Group = "Test",
            Description = "Test-Console",
            Id = Guid.NewGuid(),
            JobType = JobType.Period,
            Priority = Volo.Abp.BackgroundJobs.BackgroundJobPriority.Low,
            Cron = "0/5 * * * * ? ",
            TryCount = 10,
            Status = JobStatus.Running,
            // Define this field to handle concurrency
            LockTimeOut = 120,
        });

        // Add a one-time task to the queue, to be executed after 10 seconds (Interval)
        await scheduler.QueueAsync(new JobInfo
        {
            Type = typeof(ConsoleJob).AssemblyQualifiedName,
            Args = new Dictionary<string, object>(),
            Name = "Test-Console-Once",
            Group = "Test",
            Description = "Test-Console",
            Id = Guid.NewGuid(),
            JobType = JobType.Once,
            Priority = Volo.Abp.BackgroundJobs.BackgroundJobPriority.Low,
            Interval = 10,
            TryCount = 10,
            Status = JobStatus.Running,
        });

        // Add a persistent task to the queue, to be executed after 10 seconds (Interval), maximum 5 executions (MaxCount)
        await scheduler.QueueAsync(new JobInfo
        {
            Type = typeof(ConsoleJob).AssemblyQualifiedName,
            Args = new Dictionary<string, object>(),
            Name = "Test-Console-Persistent",
            Group = "Test",
            Description = "Test-Console",
            Id = Guid.NewGuid(),
            JobType = JobType.Persistent,
            Priority = Volo.Abp.BackgroundJobs.BackgroundJobPriority.Low,
            Interval = 10,
            TryCount = 10,
            MaxCount = 5,
            Status = JobStatus.Running,
        });

        // You can also add framework background jobs to the job scheduler without changing usage habits
        var backgroundJobManager = ServiceProvider.GetRequiredService<IBackgroundJobManager>();
        await jobManager.EnqueueAsync(
            new SmsJobArgs
            {
                PhoneNumber = "13800138000",
                Message = "Message from framework background worker"
            },
            BackgroundJobPriority.High,
            TimeSpan.FromSeconds(10));

        // Similarly, you can add framework background workers to the job scheduler without changing usage habits
        var backgroundWorkManager = ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
        // Console output every 20 seconds
        await backgroundWorkManager.AddAsync(ServiceProvider.GetRequiredService<ConsoleWorker>());
    }
}

public class SmsJobArgs
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}

public class SmsJob : AsyncBackgroundJob<SmsJobArgs>, ITransientDependency
{
    public override Task ExecuteAsync(SmsJobArgs args)
    {
        Console.WriteLine($"Send sms message: {args.Message}");

        return Task.CompletedTask;
    }
}

public class ConsoleWorker : AsyncPeriodicBackgroundWorkerBase, ISingletonDependency
{
    public ConsoleWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory)
        : base(timer, serviceScopeFactory)
    {
        timer.Period = 20000;
    }

    protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] - ConsoleWorker Do Work.");
        return Task.CompletedTask;
    }
}
```
