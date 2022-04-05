using LINGYUN.Abp.TaskManagement;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.TaskManagement;

public class TaskManagementJobPublisher : IJobPublisher, ITransientDependency
{
    protected AbpBackgroundTasksOptions Options { get; }
    protected IBackgroundJobInfoAppService BackgroundJobAppService { get; }

    public TaskManagementJobPublisher(
        IBackgroundJobInfoAppService backgroundJobAppService,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        BackgroundJobAppService = backgroundJobAppService;
        Options = options.Value;
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
            Source = job.Source,
            Priority = job.Priority,
            NodeName = Options.NodeName,
        };

        await BackgroundJobAppService.CreateAsync(input);

        return true;
    }
}
