using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.TaskManagement.Quartz;

public class QuartzJobExecutorProvider : IQuartzJobExecutorProvider, ISingletonDependency
{
    public IJobDetail CreateJob(JobInfo job)
    {
        throw new NotImplementedException();
    }

    public ITrigger CreateTrigger(JobInfo job)
    {
        throw new NotImplementedException();
    }
}
