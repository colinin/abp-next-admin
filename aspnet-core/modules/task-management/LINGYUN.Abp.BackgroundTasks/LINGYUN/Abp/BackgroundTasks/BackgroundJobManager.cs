using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(ReplaceServices = true)]
public class BackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IClock Clock { get; }
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected AbpBackgroundJobOptions Options { get; }
    public BackgroundJobManager(
        IClock clock,
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IGuidGenerator guidGenerator,
        IJsonSerializer jsonSerializer,
        IOptions<AbpBackgroundJobOptions> options)
    {
        Clock = clock;
        JobStore = jobStore;
        JobScheduler = jobScheduler;
        GuidGenerator = guidGenerator;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
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
            { nameof(TArgs), JsonSerializer.Serialize(args) },
            { "ArgsType", jobConfiguration.ArgsType.AssemblyQualifiedName },
            { "JobType", jobConfiguration.JobType.AssemblyQualifiedName },
            { "JobName", jobConfiguration.JobName },
        };
        var jobInfo = new JobInfo
        {
            Id = jobId,
            Name = jobId.ToString(),
            Group = "BackgroundJobs",
            Priority = ConverForm(priority),
            BeginTime = DateTime.Now,
            Args = jobArgs,
            Description = "From the framework background jobs",
            JobType = JobType.Once,
            Interval = interval,
            CreationTime = Clock.Now,
            // 确保不会被轮询入队
            Status = JobStatus.None,
            Type = typeof(BackgroundJobAdapter<TArgs>).AssemblyQualifiedName,
        };

        // 存储状态
        await JobStore.StoreAsync(jobInfo);

        // 手动入队
        await JobScheduler.QueueAsync(jobInfo);

        return jobId.ToString();
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
