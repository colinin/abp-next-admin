using System;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobEventContext
{
    public IServiceProvider ServiceProvider { get; }
    public JobEventData EventData { get; }

    public JobEventContext(
        IServiceProvider serviceProvider,
        JobEventData jobEventData)
    {
        ServiceProvider = serviceProvider;
        EventData = jobEventData;
    }
}
