using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.MicroService.MessageService.BackgroundJobs;

public class NotificationPublishJob : AsyncBackgroundJob<NotificationPublishJobArgs>, ITransientDependency
{
    protected IClock Clock { get; }
    protected AbpNotificationsPublishOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected INotificationStore NotificationStore { get; }
    protected INotificationDataSerializer NotificationDataSerializer { get; }
    public NotificationPublishJob(
        IClock clock,
        IOptions<AbpNotificationsPublishOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        INotificationStore notificationStore,
        INotificationDataSerializer notificationDataSerializer)
    {
        Clock = clock;
        Options = options.Value;
        ServiceScopeFactory = serviceScopeFactory;
        NotificationStore = notificationStore;
        NotificationDataSerializer = notificationDataSerializer;
    }

    public override async Task ExecuteAsync(NotificationPublishJobArgs args)
    {
        var providerType = Type.GetType(args.ProviderType);
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            if (scope.ServiceProvider.GetRequiredService(providerType) is INotificationPublishProvider publishProvider)
            {
                var store = scope.ServiceProvider.GetRequiredService<INotificationStore>();
                var notification = await store.GetNotificationOrNullAsync(args.TenantId, args.NotificationId);
                notification.Data = NotificationDataSerializer.Serialize(notification.Data);

                var sendInfo = OnPublishing(publishProvider, notification, args.UserIdentifiers);

                try
                {
                    if (await publishProvider.CanPublishAsync(notification))
                    {
                        var context = new NotificationPublishContext(notification, args.UserIdentifiers);
                        // 发布
                        await publishProvider.PublishAsync(context);

                        sendInfo.Sent(context.Exception);

                        if (context.Exception == null && !context.Reason.IsNullOrWhiteSpace())
                        {
                            sendInfo.Cancel(context.Reason);
                        }

                        Logger.LogDebug($"Send notification {notification.Name} with provider {publishProvider.Name} was successful");
                    }
                    else
                    {
                        sendInfo.Disbaled();
                    }

                    await OnPublished(sendInfo);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Send notification error with provider {publishProvider.Name}");
                    Logger.LogWarning($"Error message:{ex.Message}");

                    try
                    {
                        sendInfo.Sent(ex);
                        await OnPublished(sendInfo);
                    }
                    catch { }

                    throw;
                }
            }
        }
    }

    protected virtual NotificationSendInfo OnPublishing(
            INotificationPublishProvider provider,
            NotificationInfo notification,
            IEnumerable<UserIdentifier> identifiers)
    {
        return new NotificationSendInfo(
            provider.Name,
            Clock.Now,
            notification,
            identifiers);
    }

    protected async Task OnPublished(NotificationSendInfo sendInfo)
    {
        await NotificationStore.InsertSendStateAsync(sendInfo);
    }
}
