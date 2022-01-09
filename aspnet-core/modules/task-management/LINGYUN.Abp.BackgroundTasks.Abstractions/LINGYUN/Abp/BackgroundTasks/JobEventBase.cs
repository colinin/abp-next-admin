using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public abstract class JobEventBase<TEvent> : IJobEvent
{
    public ILogger<TEvent> Logger { protected get; set; }
    protected JobEventBase()
    {
        Logger = NullLogger<TEvent>.Instance;
    }

    public async Task OnJobAfterExecuted(JobEventContext context)
    {
        try
        {
            await OnJobAfterExecutedAsync(context);
        }
        catch (Exception ex)
        {
            Logger.LogError("Failed to execute event, error:" + GetSourceException(ex).Message);
        }
    }

    public async Task OnJobBeforeExecuted(JobEventContext context)
    {
        try
        {
            await OnJobBeforeExecutedAsync(context);
        }
        catch (Exception ex)
        {
            Logger.LogError("Failed to execute preprocessing event, error:" + GetSourceException(ex).Message);
        }
    }

    protected virtual Task OnJobAfterExecutedAsync(JobEventContext context)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnJobBeforeExecutedAsync(JobEventContext context)
    {
        return Task.CompletedTask;
    }

    protected virtual Exception GetSourceException(Exception exception)
    {
        if (exception.InnerException != null)
        {
            return GetSourceException(exception.InnerException);
        }
        return exception;
    }
}
