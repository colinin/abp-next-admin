using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

public class JobNotifierEvent : JobEventBase<JobNotifierEvent>, ITransientDependency
{
    protected async override Task OnJobAfterExecutedAsync(JobEventContext context)
    {
        try
        {
            var notifier = context.ServiceProvider.GetRequiredService<IJobExecutedNotifier>();
            if (context.EventData.Exception != null)
            {
                await notifier.NotifyErrorAsync(context);
            }
            else
            {
                await notifier.NotifySuccessAsync(context);
            }

            await notifier.NotifyComplateAsync(context);
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"An exception thow with job notify: {ex.Message}");
        }
    }
}
