using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.BackgroundTasks.Internal;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.MultiTenancy;
using LINGYUN.Abp.Saas.Tenants;
using LY.MicroService.TaskManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.TaskManagement.EventBus.Handlers
{
    public class TenantSynchronizer : 
        IDistributedEventHandler<CreateEventData>,
        IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
        ITransientDependency
    {
        protected ILogger<TenantSynchronizer> Logger { get; }

        protected ICurrentTenant CurrentTenant { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IDbSchemaMigrator DbSchemaMigrator { get; }
        protected AbpBackgroundTasksOptions Options { get; }
        protected IJobStore JobStore { get; }
        protected IJobScheduler JobScheduler { get; }
        public TenantSynchronizer(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IDbSchemaMigrator dbSchemaMigrator,
            IOptions<AbpBackgroundTasksOptions> options,
            IJobStore jobStore,
            IJobScheduler jobScheduler,
            ILogger<TenantSynchronizer> logger)
        {
            CurrentTenant = currentTenant;
            UnitOfWorkManager = unitOfWorkManager;
            DbSchemaMigrator = dbSchemaMigrator;
            JobStore = jobStore;
            JobScheduler = jobScheduler;
            Options = options.Value;

            Logger = logger;
        }

        public async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
        {
            // 租户删除时移除轮询作业
            var pollingJob = BuildPollingJobInfo(eventData.Entity.Id, eventData.Entity.Name);
            await JobScheduler.RemoveAsync(pollingJob);
            await JobStore.RemoveAsync(pollingJob.Id);

            var cleaningJob = BuildCleaningJobInfo(eventData.Entity.Id, eventData.Entity.Name);
            await JobScheduler.RemoveAsync(cleaningJob);
            await JobStore.RemoveAsync(cleaningJob.Id);
        }

        public async Task HandleEventAsync(CreateEventData eventData)
        {
            await MigrateAsync(eventData);

            // 持久层介入之后提供对于租户的后台工作者轮询作业
            await QueueBackgroundJobAsync(eventData);
        }

        private async Task QueueBackgroundJobAsync(CreateEventData eventData)
        {
            var pollingJob = BuildPollingJobInfo(eventData.Id, eventData.Name);
            await JobStore.StoreAsync(pollingJob);
            await JobScheduler.QueueAsync(pollingJob);

            var cleaningJob = BuildCleaningJobInfo(eventData.Id, eventData.Name);
            await JobStore.StoreAsync(cleaningJob);
            await JobScheduler.QueueAsync(cleaningJob);
        }

        private async Task MigrateAsync(CreateEventData eventData)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(eventData.Id, eventData.Name))
                {
                    Logger.LogInformation("Migrating the new tenant database with localization..");
                    // 迁移租户数据
                    await DbSchemaMigrator.MigrateAsync<TaskManagementMigrationsDbContext>(
                        (connectionString, builder) =>
                        {
                            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                            return new TaskManagementMigrationsDbContext(builder.Options);
                        });
                    await unitOfWork.SaveChangesAsync();

                    Logger.LogInformation("Migrated the new tenant database with localization.");
                }
            }
        }

        private JobInfo BuildPollingJobInfo(Guid tenantId, string tenantName)
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

        private JobInfo BuildCleaningJobInfo(Guid tenantId, string tenantName)
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
    }
}
