using LINGYUN.Abp.TaskManagement;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.TaskManagement;

public class TaskManagementJobPublisher : IJobPublisher, ITransientDependency
{
    protected IBackgroundJobInfoAppService BackgroundJobAppService { get; }

    public TaskManagementJobPublisher(
        IBackgroundJobInfoAppService backgroundJobAppService)
    {
        BackgroundJobAppService = backgroundJobAppService;
    }

    public async virtual Task<bool> PublishAsync(JobInfo job, CancellationToken cancellationToken = default)
    {
        var input = new BackgroundJobInfoCreateDto
        {
            Cron = job.Cron,
            MaxCount = job.MaxCount,
            MaxTryCount = job.MaxTryCount,
            Args = new ExtraPropertyDictionary(job.Args),
            BeginTime = job.BeginTime,
            Description = job.Description,
            EndTime = job.EndTime,
            Group = job.Group,
            Interval = job.Interval,
            Type = job.Type,
            JobType = job.JobType,
            LockTimeOut = job.LockTimeOut,
            IsEnabled = true,
            Name = job.Name,
            Priority = job.Priority,
        };

        await BackgroundJobAppService.CreateAsync(input);

        return true;
    }
}
