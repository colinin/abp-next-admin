using LINGYUN.Abp.BackgroundTasks;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobManager : DomainService
{
    protected IObjectMapper ObjectMapper { get; }
    protected IJobScheduler JobScheduler { get; }
    protected IBackgroundJobInfoRepository BackgroundJobInfoRepository { get; }

    public BackgroundJobManager(
        IObjectMapper objectMapper,
        IJobScheduler jobScheduler,
        IBackgroundJobInfoRepository backgroundJobInfoRepository)
    {
        ObjectMapper = objectMapper;
        JobScheduler = jobScheduler;
        BackgroundJobInfoRepository = backgroundJobInfoRepository;
    }

    public virtual async Task<BackgroundJobInfo> CreateAsync(BackgroundJobInfo jobInfo)
    {
        await BackgroundJobInfoRepository.InsertAsync(jobInfo);

        if (jobInfo.IsEnabled && jobInfo.JobType == JobType.Period)
        {
            var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
            await JobScheduler.QueueAsync(job);
        }

        return jobInfo;
    }

    public virtual async Task<BackgroundJobInfo> UpdateAsync(BackgroundJobInfo jobInfo, bool resetJob = false)
    {
        await BackgroundJobInfoRepository.UpdateAsync(jobInfo);

        if (!jobInfo.IsEnabled || resetJob)
        {
            var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
            await JobScheduler.RemoveAsync(job);
        }

        if (resetJob && jobInfo.JobType == JobType.Period)
        {
            await QueueAsync(jobInfo);
        }

        return jobInfo;
    }

    public virtual async Task DeleteAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.RemoveAsync(job);

        await BackgroundJobInfoRepository.DeleteAsync(jobInfo);
    }

    public virtual async Task QueueAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.QueueAsync(job);
    }

    public virtual async Task TriggerAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.TriggerAsync(job);
    }

    public virtual async Task PauseAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.PauseAsync(job);
    }

    public virtual async Task ResumeAsync(BackgroundJobInfo jobInfo)
    {
        var job = ObjectMapper.Map<BackgroundJobInfo, JobInfo>(jobInfo);
        await JobScheduler.ResumeAsync(job);
    }
}
