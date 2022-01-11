using System;
using Volo.Abp;

namespace LINGYUN.Abp.BackgroundTasks;

public class AbpJobExecutionException : AbpException
{
    public Type JobType { get; }
    /// <summary>
    /// Creates a new <see cref="AbpJobExecutionException"/> object.
    /// </summary>
    /// <param name="innerException">Inner exception</param>
    public AbpJobExecutionException(Type jobType)
        : this(
              jobType,
              $"Unable to execute job {jobType.Name}.",
              null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="AbpJobExecutionException"/> object.
    /// </summary>
    /// <param name="jobType">Execute job type</param>
    /// <param name="innerException">Inner exception</param>
    public AbpJobExecutionException(Type jobType, Exception innerException)
        : this(
            jobType,
            $"Unable to execute job {jobType.Name} because it: {innerException.Message}",
            innerException)
    {
    }
    /// <summary>
    /// Creates a new <see cref="AbpJobExecutionException"/> object.
    /// </summary>
    /// <param name="jobType">Execute job type</param>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public AbpJobExecutionException(Type jobType, string message, Exception innerException)
        : base(message, innerException)
    {
        JobType = jobType;
    }
}
