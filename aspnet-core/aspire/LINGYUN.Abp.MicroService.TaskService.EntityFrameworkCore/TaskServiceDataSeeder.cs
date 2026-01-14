using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.BackgroundTasks.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.TaskService;
public class TaskServiceDataSeeder : ITransientDependency
{
    protected AbpBackgroundTasksOptions Options { get; }
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public TaskServiceDataSeeder(
        IOptions<AbpBackgroundTasksOptions> options, 
        IJobStore jobStore, 
        IJobScheduler jobScheduler,
        ICurrentTenant currentTenant)
    {
        Options = options.Value;
        JobStore = jobStore;
        JobScheduler = jobScheduler;
        CurrentTenant = currentTenant;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId.HasValue)
        {
            using (CurrentTenant.Change(context.TenantId))
            {
                await QueueBackgroundJobAsync(context.TenantId.Value);
            }
        }
    }

    public async virtual Task RemoveSeedAsync(Guid tenantId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var pollingJob = BuildPollingJobInfo(tenantId);
            await JobScheduler.RemoveAsync(pollingJob);
            await JobStore.RemoveAsync(pollingJob.Id);

            var cleaningJob = BuildCleaningJobInfo(tenantId);
            await JobScheduler.RemoveAsync(cleaningJob);
            await JobStore.RemoveAsync(cleaningJob.Id);

            var checkingJob = BuildCheckingJobInfo(tenantId);
            await JobScheduler.RemoveAsync(checkingJob);
            await JobStore.RemoveAsync(checkingJob.Id);
        }
    }

    protected async virtual Task QueueBackgroundJobAsync(Guid tenantId)
    {
        var pollingJob = BuildPollingJobInfo(tenantId);
        await JobStore.StoreAsync(pollingJob);
        await JobScheduler.QueueAsync(pollingJob);

        var cleaningJob = BuildCleaningJobInfo(tenantId);
        await JobStore.StoreAsync(cleaningJob);
        await JobScheduler.QueueAsync(cleaningJob);

        var checkingJob = BuildCheckingJobInfo(tenantId);
        await JobStore.StoreAsync(checkingJob);
        await JobScheduler.QueueAsync(checkingJob);
    }

    protected virtual JobInfo BuildPollingJobInfo(Guid tenantId)
    {
        return new JobInfo
        {
            Id = tenantId.ToString() + "_Polling",
            Name = nameof(BackgroundPollingJob),
            Group = "Polling",
            Description = "Polling tasks to be executed",
            Args = new Dictionary<string, object>() { { nameof(JobInfo.TenantId), tenantId } },
            Status = JobStatus.Running,
            BeginTime = DateTime.Now,
            CreationTime = DateTime.Now,
            Cron = Options.JobFetchCronExpression,
            JobType = JobType.Period,
            Priority = JobPriority.High,
            Source = JobSource.System,
            LockTimeOut = Options.JobFetchLockTimeOut,
            TenantId = tenantId,
            Type = typeof(BackgroundPollingJob).AssemblyQualifiedName,
        };
    }

    protected virtual JobInfo BuildCleaningJobInfo(Guid tenantId)
    {
        return new JobInfo
        {
            Id = tenantId.ToString() + "_Cleaning",
            Name = nameof(BackgroundCleaningJob),
            Group = "Cleaning",
            Description = "Cleaning tasks to be executed",
            Args = new Dictionary<string, object>() { { nameof(JobInfo.TenantId), tenantId } },
            Status = JobStatus.Running,
            BeginTime = DateTime.Now,
            CreationTime = DateTime.Now,
            Cron = Options.JobCleanCronExpression,
            JobType = JobType.Period,
            Priority = JobPriority.High,
            Source = JobSource.System,
            TenantId = tenantId,
            Type = typeof(BackgroundCleaningJob).AssemblyQualifiedName,
        };
    }

    protected virtual JobInfo BuildCheckingJobInfo(Guid tenantId)
    {
        return new JobInfo
        {
            Id = tenantId.ToString() + "_Checking",
            Name = nameof(BackgroundCheckingJob),
            Group = "Checking",
            Description = "Checking tasks to be executed",
            Args = new Dictionary<string, object>() { { nameof(JobInfo.TenantId), tenantId } },
            Status = JobStatus.Running,
            BeginTime = DateTime.Now,
            CreationTime = DateTime.Now,
            Cron = Options.JobCheckCronExpression,
            LockTimeOut = Options.JobCheckLockTimeOut,
            JobType = JobType.Period,
            Priority = JobPriority.High,
            Source = JobSource.System,
            TenantId = tenantId,
            Type = typeof(BackgroundCheckingJob).AssemblyQualifiedName,
        };
    }
}
