using LINGYUN.Abp.BackgroundTasks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.TaskManagement;

[Dependency(ReplaceServices = true)]
public class BackgroundJobStore : IJobStore, ITransientDependency
{
    protected IObjectMapper ObjectMapper { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IBackgroundJobInfoRepository JobInfoRepository { get; }
    protected IBackgroundJobLogRepository JobLogRepository { get; }

    protected AbpBackgroundTasksOptions Options { get; }

    public BackgroundJobStore(
        IObjectMapper objectMapper,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IBackgroundJobInfoRepository jobInfoRepository,
        IBackgroundJobLogRepository jobLogRepository,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        ObjectMapper = objectMapper;
        CurrentTenant = currentTenant;
        UnitOfWorkManager = unitOfWorkManager;
        JobInfoRepository = jobInfoRepository;
        JobLogRepository = jobLogRepository;
        Options = options.Value;
    }

    public async virtual Task<List<JobInfo>> GetAllPeriodTasksAsync(CancellationToken cancellationToken = default)
    {
        var jobInfos = await JobInfoRepository.GetAllPeriodTasksAsync(Options.NodeName, cancellationToken);

        return ObjectMapper.Map<List<BackgroundJobInfo>, List<JobInfo>>(jobInfos);
    }

    public async virtual Task<List<JobInfo>> GetWaitingListAsync(int maxResultCount, CancellationToken cancellationToken = default)
    {
        var jobInfos = await JobInfoRepository.GetWaitingListAsync(Options.NodeName, maxResultCount, cancellationToken);

        return ObjectMapper.Map<List<BackgroundJobInfo>, List<JobInfo>>(jobInfos);
    }

    public async virtual Task<JobInfo> FindAsync(string jobId)
    {
        return await JobInfoRepository.FindJobAsync(jobId);
    }

    public async virtual Task StoreAsync(JobInfo jobInfo)
    {
        using var unitOfWork = UnitOfWorkManager.Begin();
        using (CurrentTenant.Change(jobInfo.TenantId))
        {
            var backgroundJobInfo = await JobInfoRepository.FindAsync(jobInfo.Id);
            if (backgroundJobInfo != null)
            {
                backgroundJobInfo.SetNextRunTime(jobInfo.NextRunTime);
                backgroundJobInfo.SetLastRunTime(jobInfo.LastRunTime);
                backgroundJobInfo.SetStatus(jobInfo.Status);
                backgroundJobInfo.SetResult(jobInfo.Result);
                backgroundJobInfo.TriggerCount = jobInfo.TriggerCount;
                backgroundJobInfo.TryCount = jobInfo.TryCount;
                backgroundJobInfo.IsAbandoned = jobInfo.IsAbandoned;

                await JobInfoRepository.UpdateAsync(backgroundJobInfo);
            }
            else
            {
                backgroundJobInfo = new BackgroundJobInfo(
                    jobInfo.Id,
                    jobInfo.Name,
                    jobInfo.Group,
                    jobInfo.Type,
                    jobInfo.Args,
                    jobInfo.BeginTime,
                    jobInfo.EndTime,
                    jobInfo.Priority,
                    jobInfo.Source,
                    jobInfo.MaxCount,
                    jobInfo.MaxTryCount,
                    jobInfo.NodeName,
                    jobInfo.TenantId)
                {
                    IsEnabled = true,
                    TriggerCount = jobInfo.TriggerCount,
                    IsAbandoned = jobInfo.IsAbandoned,
                    TryCount = jobInfo.TryCount,
                    LockTimeOut = jobInfo.LockTimeOut,
                    Description = jobInfo.Description
                };
                backgroundJobInfo.SetNextRunTime(jobInfo.NextRunTime);
                backgroundJobInfo.SetLastRunTime(jobInfo.LastRunTime);
                backgroundJobInfo.SetStatus(jobInfo.Status);
                backgroundJobInfo.SetResult(jobInfo.Result);
                switch (jobInfo.JobType)
                {
                    case JobType.Once:
                        backgroundJobInfo.SetOnceJob(jobInfo.Interval);
                        break;
                    case JobType.Persistent:
                        backgroundJobInfo.SetPersistentJob(jobInfo.Interval);
                        break;
                    case JobType.Period:
                        backgroundJobInfo.SetPeriodJob(jobInfo.Cron);
                        break;
                }

                await JobInfoRepository.InsertAsync(backgroundJobInfo);
            }
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task RemoveAsync(string jobId)
    {
        using var unitOfWork = UnitOfWorkManager.Begin();
        await JobInfoRepository.DeleteAsync(jobId);

        await unitOfWork.SaveChangesAsync();
    }

    public async virtual Task StoreLogAsync(JobEventData eventData)
    {
        using var unitOfWork = UnitOfWorkManager.Begin();
        using (CurrentTenant.Change(eventData.TenantId))
        {
            var jogLog = new BackgroundJobLog(
            eventData.Type.Name,
            eventData.Group,
            eventData.Name,
            eventData.RunTime,
            eventData.TenantId)
            {
                JobId = eventData.Key
            };

            jogLog.SetMessage(
                eventData.Exception == null ? eventData.Result ?? "OK" : GetSourceException(eventData.Exception).Message,
                eventData.Exception);

            await JobLogRepository.InsertAsync(jogLog);

            await unitOfWork.SaveChangesAsync();
        }
    }

    public async virtual Task CleanupAsync(
        int maxResultCount,
        TimeSpan jobExpiratime,
        CancellationToken cancellationToken = default)
    {
        using var unitOfWork = UnitOfWorkManager.Begin();
        var jobs = await JobInfoRepository.GetExpiredJobsAsync(
            Options.NodeName,
            maxResultCount,
            jobExpiratime,
            cancellationToken);

        await JobInfoRepository.DeleteManyAsync(jobs, cancellationToken: cancellationToken);

        await unitOfWork.SaveChangesAsync();
    }

    protected virtual Exception GetSourceException(Exception exception)
    {
        if (exception.InnerException != null)
        {
            return GetSourceException(exception.InnerException);
        }
        return exception;
    }
}
