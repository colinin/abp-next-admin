using LINGYUN.Abp.BackgroundTasks;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobManager : DomainService
{
    protected IObjectMapper ObjectMapper { get; }
    protected IJobScheduler JobScheduler { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IBackgroundJobInfoRepository BackgroundJobInfoRepository { get; }

    public BackgroundJobManager(
        IObjectMapper objectMapper,
        IJobScheduler jobScheduler,
        IUnitOfWorkManager unitOfWorkManager,
        IBackgroundJobInfoRepository backgroundJobInfoRepository)
    {
        ObjectMapper = objectMapper;
        JobScheduler = jobScheduler;
        UnitOfWorkManager = unitOfWorkManager;
        BackgroundJobInfoRepository = backgroundJobInfoRepository;
    }

    public virtual async Task<BackgroundJobInfo> CreateAsync(BackgroundJobInfo jobInfo)
    {
        await BackgroundJobInfoRepository.InsertAsync(jobInfo);

        if (jobInfo.IsEnabled && jobInfo.JobType == JobType.Period)
        {
            var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
            if (await JobScheduler.ExistsAsync(job))
            {
                throw new BusinessException(TaskManagementErrorCodes.JobNameAlreadyExists)
                    .WithData("Group", job.Group)
                    .WithData("Name", job.Name);
            }
            UnitOfWorkManager.Current.OnCompleted(async () =>
            {
                await JobScheduler.QueueAsync(job);
            });
        }

        return jobInfo;
    }

    public virtual async Task<BackgroundJobInfo> UpdateAsync(BackgroundJobInfo jobInfo, bool resetJob = false)
    {
        await BackgroundJobInfoRepository.UpdateAsync(jobInfo);

        if (!jobInfo.IsEnabled || resetJob)
        {
            UnitOfWorkManager.Current.OnCompleted(async () =>
            {
                var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
                await JobScheduler.RemoveAsync(job);
            });
        }

        if (resetJob && jobInfo.JobType == JobType.Period)
        {
            UnitOfWorkManager.Current.OnCompleted(async () =>
            {
                await QueueAsync(jobInfo);
            });
        }

        return jobInfo;
    }

    public virtual async Task DeleteAsync(BackgroundJobInfo jobInfo)
    {
        await BackgroundJobInfoRepository.DeleteAsync(jobInfo);

        UnitOfWorkManager.Current.OnCompleted(async () =>
        {
            var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
            await JobScheduler.RemoveAsync(job);
        });
    }

    public virtual async Task QueueAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.QueueAsync(job);
    }

    public virtual async Task TriggerAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        job.JobType = JobType.Once;
        // 延迟两秒触发
        job.Interval = 2;

        await JobScheduler.TriggerAsync(job);
    }

    public virtual async Task PauseAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.PauseAsync(job);

        jobInfo.SetStatus(JobStatus.Paused);
        jobInfo.SetNextRunTime(null);

        await BackgroundJobInfoRepository.UpdateAsync(jobInfo);
    }

    public virtual async Task ResumeAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.ResumeAsync(job);

        jobInfo.SetStatus(JobStatus.Running);
        jobInfo.IsAbandoned = false;
        jobInfo.IsEnabled = true;

        await BackgroundJobInfoRepository.UpdateAsync(jobInfo);
    }

    public virtual async Task StopAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.RemoveAsync(job);

        jobInfo.SetStatus(JobStatus.Stopped);
        jobInfo.SetNextRunTime(null);

        await BackgroundJobInfoRepository.UpdateAsync(jobInfo);
    }
}
