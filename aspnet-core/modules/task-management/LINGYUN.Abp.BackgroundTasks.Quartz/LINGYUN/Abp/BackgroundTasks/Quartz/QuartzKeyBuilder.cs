using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzKeyBuilder : IQuartzKeyBuilder, ISingletonDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    public QuartzKeyBuilder(ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;
    }

    public JobKey CreateJobKey(JobInfo jobInfo)
    {
        var name = jobInfo.Id;
        var group = CurrentTenant.IsAvailable
            ? $"{CurrentTenant.Id}:{jobInfo.Group}"
            : $"Default:{jobInfo.Group}";

        return new JobKey(name, group);
    }

    public TriggerKey CreateTriggerKey(JobInfo jobInfo)
    {
        var name = jobInfo.Id;
        var group = CurrentTenant.IsAvailable
            ? $"{CurrentTenant.Id}:{jobInfo.Group}"
            : $"Default:{jobInfo.Group}";

        return new TriggerKey(name, group);
    }
}
