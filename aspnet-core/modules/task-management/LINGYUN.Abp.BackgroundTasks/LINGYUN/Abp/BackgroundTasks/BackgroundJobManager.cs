using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(ReplaceServices = true)]
public class BackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IClock Clock { get; }
    protected IJobStore JobStore { get; }
    protected IJobPublisher JobPublisher { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected AbpBackgroundTasksOptions TasksOptions { get; }
    protected AbpBackgroundJobOptions Options { get; }
    public BackgroundJobManager(
        IClock clock,
        IJobStore jobStore,
        IJobPublisher jobPublisher,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IOptions<AbpBackgroundJobOptions> options,
        IOptions<AbpBackgroundTasksOptions> taskOptions)
    {
        Clock = clock;
        JobStore = jobStore;
        JobPublisher = jobPublisher;
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        Options = options.Value;
        TasksOptions = taskOptions.Value;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(
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
            { nameof(TArgs), JsonConvert.SerializeObject(args) },
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
