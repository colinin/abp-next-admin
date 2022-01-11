using LINGYUN.Abp.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.TaskManagement;

[Dependency(ReplaceServices = true)]
public class BackgroundJobStore : IJobStore, ITransientDependency
{
    protected IObjectMapper ObjectMapper { get; }
    protected IBackgroundJobInfoRepository JobInfoRepository { get; }
    protected IBackgroundJobLogRepository JobLogRepository { get; }

    public BackgroundJobStore(
        IObjectMapper objectMapper,
        IBackgroundJobInfoRepository jobInfoRepository,
        IBackgroundJobLogRepository jobLogRepository)
    {
        ObjectMapper = objectMapper;
        JobInfoRepository = jobInfoRepository;
        JobLogRepository = jobLogRepository;
    }

    public async virtual Task<List<JobInfo>> GetAllPeriodTasksAsync(CancellationToken cancellationToken = default)
    {
        var jobInfos = await JobInfoRepository.GetAllPeriodTasksAsync(cancellationToken);

        return ObjectMapper.Map<List<BackgroundJobInfo>, List<JobInfo>>(jobInfos);
    }

    public async virtual Task<List<JobInfo>> GetWaitingListAsync(int maxResultCount, CancellationToken cancellationToken = default)
    {
        var jobInfos = await JobInfoRepository.GetWaitingListAsync(maxResultCount, cancellationToken);

        return ObjectMapper.Map<List<BackgroundJobInfo>, List<JobInfo>>(jobInfos);
    }

    public async virtual Task<JobInfo> FindAsync(Guid jobId)
    {
        var jobInfo = await JobInfoRepository.FindAsync(jobId);

        return ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
    }

    [UnitOfWork]
    public async virtual Task StoreAsync(JobInfo jobInfo)
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
                jobInfo.MaxCount,
                jobInfo.MaxTryCount)
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
    }

    [UnitOfWork]
    public async virtual Task StoreLogAsync(JobEventData eventData)
    {
        var jogLog = new BackgroundJobLog(
            eventData.Type.Name, 
            eventData.Group,
            eventData.Name,
            eventData.RunTime)
        {
            JobId = eventData.Key
        };

        jogLog.SetMessage(
            eventData.Exception == null ? eventData.Result ?? "OK" : "Failed",
            eventData.Exception);

        await JobLogRepository.InsertAsync(jogLog);
    }

    [UnitOfWork]
    public async virtual Task CleanupAsync(
        int maxResultCount,
        TimeSpan jobExpiratime,
        CancellationToken cancellationToken = default)
    {
        var jobs = await JobInfoRepository.GetExpiredJobsAsync(
            maxResultCount,
            jobExpiratime,
            cancellationToken);

        await JobInfoRepository.DeleteManyAsync(jobs, cancellationToken: cancellationToken);
    }
}
