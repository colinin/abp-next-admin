using Quartz;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public interface IQuartzJobCreator
{
    IJobDetail? CreateJob(JobInfo job);

    ITrigger? CreateTrigger(JobInfo job);
}
