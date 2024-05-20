using System;

namespace LINGYUN.Abp.BackgroundTasks;
public interface IJobExceptionTypeFinder
{
    JobExceptionType GetExceptionType(JobEventContext eventContext, Exception exception);
}

