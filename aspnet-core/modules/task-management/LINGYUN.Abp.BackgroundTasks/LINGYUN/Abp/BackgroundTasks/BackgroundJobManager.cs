using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(ReplaceServices = true)]
public class BackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IClock Clock { get; }
    protected IJobStore JobStore { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected AbpBackgroundJobOptions Options { get; }
    public BackgroundJobManager(
        IClock clock,
        IJobStore jobStore,
        IGuidGenerator guidGenerator,
        IOptions<AbpBackgroundJobOptions> options)
    {
        Clock = clock;
        JobStore = jobStore;
        GuidGenerator = guidGenerator;
        Options = options.Value;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        var jobConfiguration = Options.GetJob<TArgs>();
        var interval = 60;
        if (delay.HasValue)
        {
            interval = delay.Value.Seconds;
        }
        var jobId = GuidGenerator.Create();
        var jobArgs = new Dictionary<string, object>
        {
            { nameof(TArgs), args },
            { "ArgsType", jobConfiguration.ArgsType.AssemblyQualifiedName },
            { "JobType", typeof(BackgroundJobAdapter<TArgs>).AssemblyQualifiedName },
        };
        var jobInfo = new JobInfo
        {
            Id = jobId,
            Name = jobConfiguration.JobName,
            Group = "BackgroundJobs",
            Priority = ConverForm(priority),
            BeginTime = DateTime.Now,
            Args = jobArgs,
            Description = "From the framework background jobs",
            JobType = JobType.Once,
            Interval = interval,
            CreationTime = Clock.Now,
            Status = JobStatus.Running,
            Type = typeof(BackgroundJobAdapter<TArgs>).AssemblyQualifiedName,
        };

        // 作为一次性任务持久化
        await JobStore.StoreAsync(jobInfo);

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
