using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications.Internal
{
    internal class NotificationCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
    {
        protected AbpNotificationCleanupOptions Options { get; }

        public NotificationCleanupBackgroundWorker(
            AbpAsyncTimer timer, 
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpNotificationCleanupOptions> options) 
            : base(timer, serviceScopeFactory)
        {
            Options = options.Value;
            timer.Period = Options.CleanupPeriod;
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            try
            {
                var store = workerContext.ServiceProvider.GetRequiredService<INotificationStore>();
                Logger.LogDebug("Before cleanup expiration jobs...");
                await store.DeleteNotificationAsync(Options.CleanupBatchSize);
                Logger.LogDebug("Expiration jobs cleanup job was successful...");
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Expiration jobs cleanup job was failed...");
                Logger.LogWarning("Error:{0}", ex.Message);
            }
        }
    }
}
