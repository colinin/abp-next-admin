using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.BackgroundTasks.Internal;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore;
public class SingleDbMigrationEventHandler : 
    EfCoreDatabaseMigrationEventHandlerBase<SingleMigrationsDbContext>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>
{
    protected AbpBackgroundTasksOptions Options { get; }
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected IDataSeeder DataSeeder { get; }

    public SingleDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager, 
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus, 
        ILoggerFactory loggerFactory,
        IDataSeeder dataSeeder,
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IOptions<AbpBackgroundTasksOptions> options) 
        : base(
            ConnectionStringNameAttribute.GetConnStringName<SingleMigrationsDbContext>(),
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
        JobStore = jobStore;
        JobScheduler = jobScheduler;
        Options = options.Value;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        // 租户删除时移除轮询作业
        var pollingJob = BuildPollingJobInfo(eventData.Entity.Id, eventData.Entity.Name);
        await JobScheduler.RemoveAsync(pollingJob);
        await JobStore.RemoveAsync(pollingJob.Id);

        var cleaningJob = BuildCleaningJobInfo(eventData.Entity.Id, eventData.Entity.Name);
        await JobScheduler.RemoveAsync(cleaningJob);
        await JobStore.RemoveAsync(cleaningJob.Id);

        var checkingJob = BuildCheckingJobInfo(eventData.Entity.Id, eventData.Entity.Name);
        await JobScheduler.RemoveAsync(checkingJob);
        await JobStore.RemoveAsync(checkingJob.Id);
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        using (CurrentTenant.Change(eventData.Id))
        {
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
            {
                await QueueBackgroundJobAsync(eventData);

                var seedContext = new DataSeedContext(eventData.Id);
                foreach (var prop in eventData.Properties)
                {
                    seedContext.WithProperty(prop.Key, prop.Value);
                }

                await DataSeeder.SeedAsync(seedContext);

                await uow.CompleteAsync();
            }
        }
    }

    protected async virtual Task QueueBackgroundJobAsync(TenantCreatedEto eventData)
    {
        var pollingJob = BuildPollingJobInfo(eventData.Id, eventData.Name);
        await JobStore.StoreAsync(pollingJob);
        await JobScheduler.QueueAsync(pollingJob);

        var cleaningJob = BuildCleaningJobInfo(eventData.Id, eventData.Name);
        await JobStore.StoreAsync(cleaningJob);
        await JobScheduler.QueueAsync(cleaningJob);

        var checkingJob = BuildCheckingJobInfo(eventData.Id, eventData.Name);
        await JobStore.StoreAsync(checkingJob);
        await JobScheduler.QueueAsync(checkingJob);
    }

    protected virtual JobInfo BuildPollingJobInfo(Guid tenantId, string tenantName)
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

    protected virtual JobInfo BuildCleaningJobInfo(Guid tenantId, string tenantName)
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

    protected virtual JobInfo BuildCheckingJobInfo(Guid tenantId, string tenantName)
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
