using Quartz;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzKeyBuilder : IQuartzKeyBuilder, ISingletonDependency
{
    public JobKey CreateJobKey(JobInfo jobInfo)
    {
        var name = jobInfo.Id;
        var group = jobInfo.TenantId.HasValue
            ? $"{jobInfo.TenantId}:{jobInfo.Group}"
            : $"Default:{jobInfo.Group}";

        return new JobKey(name, group);
    }

    public TriggerKey CreateTriggerKey(JobInfo jobInfo)
    {
        var name = jobInfo.Id;
        var group = jobInfo.TenantId.HasValue
            ? $"{jobInfo.TenantId}:{jobInfo.Group}"
            : $"Default:{jobInfo.Group}";

        return new TriggerKey(name, group);
    }
}
