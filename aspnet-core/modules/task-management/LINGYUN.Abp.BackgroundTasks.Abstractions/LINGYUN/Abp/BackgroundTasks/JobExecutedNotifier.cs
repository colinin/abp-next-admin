using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobExecutedNotifier : IJobExecutedNotifier, ISingletonDependency
{
    public ILogger<JobExecutedNotifier> Logger { protected get; set; }

    public JobExecutedNotifier()
    {
        Logger = NullLogger<JobExecutedNotifier>.Instance;
    }

    public async Task NotifyComplateAsync([NotNull] JobEventContext context)
    {
        var notifier = context.ServiceProvider.GetService<IJobCompletedNotifierProvider>();
        if (notifier != null)
        {
            try
            {
                await notifier.NotifyComplateAsync(context);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"An exception thow with job complete notify: {ex.Message}");
            }
        }
    }

    public async Task NotifyErrorAsync([NotNull] JobEventContext context)
    {
        var notifier = context.ServiceProvider.GetService<IJobFailedNotifierProvider>();
        if (notifier != null)
        {
            try
            {
                await notifier.NotifyErrorAsync(context);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"An exception thow with job error notify: {ex.Message}");
            }
        }
    }

    public async Task NotifySuccessAsync([NotNull] JobEventContext context)
    {
        var notifier = context.ServiceProvider.GetService<IJobSuccessNotifierProvider>();
        if (notifier != null)
        {
            try
            {
                await notifier.NotifySuccessAsync(context);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"An exception thow with job success notify: {ex.Message}");
            }
        }
    }
}
