using System;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobExceptionNotificationContext
{
    public JobInfo JobInfo { get; }
    public Exception Exception { get; }
    public JobExceptionNotificationContext(
        JobInfo jobInfo,
        Exception exception)
    {
        JobInfo = jobInfo;
        Exception = exception;
    }
}
