using Quartz;

namespace LINGYUN.Abp.TaskManagement.Quartz;

public interface IQuartzJobExecutorProvider
{
    IJobDetail CreateJob(JobInfo job);

    ITrigger CreateTrigger(JobInfo job);
}
