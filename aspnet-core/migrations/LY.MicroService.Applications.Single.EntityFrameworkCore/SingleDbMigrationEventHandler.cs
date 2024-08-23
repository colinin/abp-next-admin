using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.BackgroundTasks.Internal;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore;
public class SingleDbMigrationEventHandler : 
    EfCoreDatabaseMigrationEventHandlerBase<SingleMigrationsDbContext>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>
{
    protected AbpBackgroundTasksOptions Options { get; }
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IdentityUserManager IdentityUserManager { get; }
    protected IdentityRoleManager IdentityRoleManager { get; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }

    public SingleDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager, 
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus, 
        ILoggerFactory loggerFactory,
        IGuidGenerator guidGenerator,
        IdentityUserManager identityUserManager,
        IdentityRoleManager identityRoleManager,
        IPermissionDataSeeder permissionDataSeeder,
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IOptions<AbpBackgroundTasksOptions> options) 
        : base("SingleDbMigrator", currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        GuidGenerator = guidGenerator;
        IdentityUserManager = identityUserManager;
        IdentityRoleManager = identityRoleManager;
        PermissionDataSeeder = permissionDataSeeder;
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
        if (!schemaMigrated)
        {
            return;
        }

        using (CurrentTenant.Change(eventData.Id))
        {
            await QueueBackgroundJobAsync(eventData);

            await SeedTenantDefaultRoleAsync(eventData);
            await SeedTenantAdminAsync(eventData);
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

    protected async virtual Task SeedTenantDefaultRoleAsync(TenantCreatedEto eventData)
    {
        // 默认用户
        var roleId = GuidGenerator.Create();
        var defaultRole = new IdentityRole(roleId, "Users", eventData.Id)
        {
            IsStatic = true,
            IsPublic = true,
            IsDefault = true,
        };
        (await IdentityRoleManager.CreateAsync(defaultRole)).CheckErrors();

        // 所有用户都应该具有查询用户权限, 用于IM场景
        await PermissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            defaultRole.Name,
            new string[]
            {
                IdentityPermissions.UserLookup.Default,
                IdentityPermissions.Users.Default
            },
            tenantId: eventData.Id);
    }

    protected async virtual Task SeedTenantAdminAsync(TenantCreatedEto eventData)
    {
        const string tenantAdminUserName = "admin";
        const string tenantAdminRoleName = "admin";
        Guid tenantAdminRoleId;
        if (!await IdentityRoleManager.RoleExistsAsync(tenantAdminRoleName))
        {
            tenantAdminRoleId = GuidGenerator.Create();
            var tenantAdminRole = new IdentityRole(tenantAdminRoleId, tenantAdminRoleName, eventData.Id)
            {
                IsStatic = true,
                IsPublic = true
            };
            (await IdentityRoleManager.CreateAsync(tenantAdminRole)).CheckErrors();
        }
        else
        {
            var tenantAdminRole = await IdentityRoleManager.FindByNameAsync(tenantAdminRoleName);
            tenantAdminRoleId = tenantAdminRole.Id;
        }

        var adminUserId = GuidGenerator.Create();
        if (eventData.Properties.TryGetValue("AdminUserId", out var userIdString) &&
            Guid.TryParse(userIdString, out var adminUserGuid))
        {
            adminUserId = adminUserGuid;
        }
        var adminEmailAddress = eventData.Properties.GetOrDefault("AdminEmail") ?? "admin@abp.io";
        var adminPassword = eventData.Properties.GetOrDefault("AdminPassword") ?? "1q2w3E*";

        var tenantAdminUser = await IdentityUserManager.FindByNameAsync(adminEmailAddress);
        if (tenantAdminUser == null)
        {
            tenantAdminUser = new IdentityUser(
                adminUserId,
                tenantAdminUserName,
                adminEmailAddress,
                eventData.Id);

            tenantAdminUser.AddRole(tenantAdminRoleId);

            // 创建租户管理用户
            (await IdentityUserManager.CreateAsync(tenantAdminUser)).CheckErrors();
            (await IdentityUserManager.AddPasswordAsync(tenantAdminUser, adminPassword)).CheckErrors();
        }
    }
}
