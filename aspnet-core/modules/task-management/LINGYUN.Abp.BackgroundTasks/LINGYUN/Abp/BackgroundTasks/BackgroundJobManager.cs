using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks;

public class BackgroundJobManager : IBackgroundJobManager
{
    protected IClock Clock { get; }
    protected IJobStore JobStore { get; }
    protected IJobPublisher JobPublisher { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected AbpBackgroundTasksOptions TasksOptions { get; }
    protected AbpBackgroundJobOptions Options { get; }
    public BackgroundJobManager(
        IClock clock,
        IJobStore jobStore,
        IJobPublisher jobPublisher,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IJsonSerializer jsonSerializer,
        IOptions<AbpBackgroundJobOptions> options,
        IOptions<AbpBackgroundTasksOptions> taskOptions)
    {
        Clock = clock;
        JobStore = jobStore;
        JobPublisher = jobPublisher;
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
        TasksOptions = taskOptions.Value;
    }

    public async virtual Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        var jobConfiguration = Options.GetJob<TArgs>();
        var interval = 0;
        if (delay.HasValue)
        {
            interval = delay.Value.Seconds;
        }
        var jobId = GuidGenerator.Create();
        var jobArgs = new Dictionary<string, object>
        {
            { nameof(TArgs), JsonSerializer.Serialize(args) },
            { "ArgsType", jobConfiguration.ArgsType.AssemblyQualifiedName },
            { "JobType", jobConfiguration.JobType.AssemblyQualifiedName },
            { "JobName", jobConfiguration.JobName },
        };
        var jobInfo = new JobInfo
        {
            Id = jobId.ToString(),
            TenantId = CurrentTenant.Id,
            Name = jobId.ToString(),
            Group = "BackgroundJobs",
            Priority = ConverForm(priority),
            Source = JobSource.System,
            BeginTime = DateTime.Now,
            Args = jobArgs,
            Description = "From the framework background jobs",
            JobType = JobType.Once,
            Interval = interval,
            CreationTime = Clock.Now,
            // 确保不会被轮询入队
            Status = JobStatus.None,
            NodeName = TasksOptions.NodeName,
            Type = typeof(BackgroundJobAdapter<TArgs>).AssemblyQualifiedName,
        };

        if (TasksOptions.JobDispatcherSelectors.IsMatch(jobConfiguration.JobType))
        {
            var selector = TasksOptions
                .JobDispatcherSelectors
                .FirstOrDefault(x => x.Predicate(jobConfiguration.JobType));

            jobInfo.Interval = selector.Interval ?? jobInfo.Interval;
            jobInfo.LockTimeOut = selector.LockTimeOut ?? jobInfo.LockTimeOut;
            jobInfo.Priority = selector.Priority ?? jobInfo.Priority;
            jobInfo.MaxCount = selector.MaxCount ?? jobInfo.MaxCount;
            jobInfo.MaxTryCount = selector.MaxTryCount ?? jobInfo.MaxTryCount;

            if (!selector.NodeName.IsNullOrWhiteSpace())
            {
                jobInfo.NodeName = selector.NodeName;
            }

            if (!selector.Cron.IsNullOrWhiteSpace())
            {
                jobInfo.Cron = selector.Cron;
            }
        }

        // 存储状态
        await JobStore.StoreAsync(jobInfo);

        // 发布作业
        await JobPublisher.PublishAsync(jobInfo);

        return jobInfo.Id;
    }

    private JobPriority ConverForm(BackgroundJobPriority priority)
    {
        return priority switch
        {
            BackgroundJobPriority.Low => JobPriority.Low,
            BackgroundJobPriority.High => JobPriority.High,
            BackgroundJobPriority.BelowNormal => JobPriority.BelowNormal,
            BackgroundJobPriority.AboveNormal => JobPriority.AboveNormal,
            _ => JobPriority.Normal,
        };
    }
}
