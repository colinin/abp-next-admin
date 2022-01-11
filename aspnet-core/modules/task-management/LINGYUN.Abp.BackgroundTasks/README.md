# LINGYUN.Abp.BackgroundTasks

后台任务（队列）模块，Abp提供的后台作业与后台工作者不支持Cron表达式, 提供可管理的后台任务（队列）功能. 

实现了**Volo.Abp.BackgroundJobs.IBackgroundJobManager**, 意味着您也能通过框架后台作业接口添加新作业.  

## 任务类别  

* JobType.Once:         一次性任务, 此类型只会被执行一次, 适用于邮件通知等场景  
* JobType.Period:       周期性任务, 此类型任务会根据Cron表达式来决定运行方式, 适用于报表分析等场景  
* JobType.Persistent:   持续性任务, 此类型任务按照给定重复次数、重复间隔运行, 适用于接口压测等场景  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpBackgroundTasksModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

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

        // 将周期性（5秒一次）任务添加到队列
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
            // 定义此字段处理并发
            LockTimeOut = 120,
        });

        // 将一次性任务添加到队列, 将在10（Interval）秒后被执行
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

        // 将持续性任务添加到队列, 将在10（Interval）秒后被执行, 最大执行5（MaxCount）次
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

        // 同样可以把框架后台作业添加到作业调度器中, 不需要更改使用习惯
        var backgroundJobManager = ServiceProvider.GetRequiredService<IBackgroundJobManager>();
        await jobManager.EnqueueAsync(
            new SmsJobArgs
            {
                PhoneNumber = "13800138000",
                Message = "来自框架的后台工作者"
            },
            BackgroundJobPriority.High,
            TimeSpan.FromSeconds(10));
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
```
