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
         // 用 BeginTime 表达延迟启动，避免与 JobInfo.Interval（重复间隔）语义混用。
         // 原实现使用 delay.Value.Seconds（仅取秒分量 0-59），
         // 会让 TimeSpan.FromMinutes(5) 这类整分钟/整小时的延迟退化为 0，导致任务立即执行。
         var now = Clock.Now;
         var beginTime = now;
         if (delay.HasValue && delay.Value > TimeSpan.Zero)
         {
             beginTime = now.Add(delay.Value);
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
             BeginTime = beginTime,
             Args = jobArgs,
             Description = "From the framework background jobs",
             JobType = JobType.Once,
             CreationTime = now,
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
