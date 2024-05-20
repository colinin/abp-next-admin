using Microsoft.Extensions.Options;
using System;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.BackgroundTasks;

public class DefaultJobExceptionTypeFinder : IJobExceptionTypeFinder, ITransientDependency
{
    protected AbpBackgroundTasksOptions Options { get; }

    public DefaultJobExceptionTypeFinder(
        IOptions<AbpBackgroundTasksOptions> options)
    {
        Options = options.Value;
    }

    public JobExceptionType GetExceptionType(JobEventContext eventContext, Exception exception)
    {
        if (exception is IHasErrorCode exceptionWithErrorCode &&
            !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
        {
            if (Options.ErrorCodeToExceptionTypeMappings.TryGetValue(exceptionWithErrorCode.Code!, out var exceptionType))
            {
                return exceptionType;
            }
        }

        if (exception is IBusinessException)
        {
            return JobExceptionType.Business;
        }

        if (exception is AbpValidationException)
        {
            return JobExceptionType.Business;
        }

        if (exception is IHasHttpStatusCode httpStatusCode)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
            // 408 Request timeout
            // 429 Too Many Requests
            if (httpStatusCode.HttpStatusCode == 408 || httpStatusCode.HttpStatusCode == 429 || httpStatusCode.HttpStatusCode >= 500)
            {
                return JobExceptionType.Network;
            }

            return JobExceptionType.Application;
        }

        return JobExceptionType.System;
    }
}
