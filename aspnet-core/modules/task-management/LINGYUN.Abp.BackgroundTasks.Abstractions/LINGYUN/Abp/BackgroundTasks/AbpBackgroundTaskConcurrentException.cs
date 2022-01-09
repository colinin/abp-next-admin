using System;
using Volo.Abp;

namespace LINGYUN.Abp.BackgroundTasks;

public class AbpBackgroundTaskConcurrentException : AbpException
{
    public Type JobType { get; }
    /// <summary>
    /// Creates a new <see cref="AbpBackgroundTaskConcurrentException"/> object.
    /// </summary>
    /// <param name="innerException">Inner exception</param>
    public AbpBackgroundTaskConcurrentException(Type jobType)
        : this(
              jobType,
              $"This job {jobType.Name} cannot be performed because it has been locked by another performer",
              null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="AbpBackgroundTaskConcurrentException"/> object.
    /// </summary>
    /// <param name="jobType">Execute job type</param>
    /// <param name="innerException">Inner exception</param>
    public AbpBackgroundTaskConcurrentException(Type jobType, Exception innerException)
        : this(
            jobType,
            $"This job {jobType.Name} cannot be performed because it has been locked by another performer",
            innerException)
    {
    }

    /// <summary>
    /// Creates a new <see cref="AbpBackgroundTaskConcurrentException"/> object.
    /// </summary>
    /// <param name="jobType">Execute job type</param>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public AbpBackgroundTaskConcurrentException(Type jobType, string message, Exception innerException)
        : base(message, innerException)
    {
        JobType = jobType;
    }
}
