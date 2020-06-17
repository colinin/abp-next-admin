using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.BackgroundJobs
{
    internal class NotificationCleanupExpritionJob : AsyncBackgroundJob<NotificationCleanupExpritionJobArgs>, ITransientDependency
    {
        protected INotificationStore Store { get; }
        protected IServiceProvider ServiceProvider { get; }

        public NotificationCleanupExpritionJob(
            INotificationStore store,
            IServiceProvider serviceProvider)
        {
            Store = store;
            ServiceProvider = serviceProvider;
        }

        public override async Task ExecuteAsync(NotificationCleanupExpritionJobArgs args)
        {
            try
            {
                Logger.LogDebug("Before cleanup exprition jobs...");
                await Store.DeleteNotificationAsync(args.Count);
                Logger.LogDebug("Exprition jobs cleanup job was successful...");
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Exprition jobs cleanup job was failed...");
                Logger.LogWarning("Error:{0}", ex.Message);
            }
        }
    }
}
