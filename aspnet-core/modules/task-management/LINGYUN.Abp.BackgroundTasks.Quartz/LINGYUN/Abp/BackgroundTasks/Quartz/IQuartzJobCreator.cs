using Quartz;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public interface IQuartzJobCreator
{
#nullable enable
    IJobDetail? CreateJob(JobInfo job);

    ITrigger? CreateTrigger(JobInfo job);
#nullable disable
}
