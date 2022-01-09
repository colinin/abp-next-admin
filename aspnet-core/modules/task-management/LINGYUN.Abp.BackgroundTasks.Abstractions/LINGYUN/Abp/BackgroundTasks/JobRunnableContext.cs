using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobRunnableContext
{
    public Type JobType { get; }
    public IServiceProvider ServiceProvider { get; }
    public IReadOnlyDictionary<string, object> JobData { get; }
    public object Result { get; private set; }
    public JobRunnableContext(
        Type jobType,
        IServiceProvider serviceProvider,
        IReadOnlyDictionary<string, object> jobData)
    {
        JobType = jobType;
        ServiceProvider = serviceProvider;
        JobData = jobData;
    }

    public void SetResult(object result)
    {
        Result = result;
    }
}
