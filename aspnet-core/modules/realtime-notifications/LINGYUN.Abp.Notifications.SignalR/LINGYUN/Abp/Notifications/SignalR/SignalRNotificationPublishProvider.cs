using LINGYUN.Abp.Notifications.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.SignalR;

public class SignalRNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.SignalR;
    public override string Name => ProviderName;

    private readonly IHubContext<NotificationsHub> _hubContext;

    private readonly AbpNotificationsSignalROptions _options;
    public SignalRNotificationPublishProvider(
        IHubContext<NotificationsHub> hubContext,
        IOptions<AbpNotificationsSignalROptions> options)
    {
        _options = options.Value;
        _hubContext = hubContext;
    }

    protected async override Task PublishAsync(
        NotificationPublishContext context,
        CancellationToken cancellationToken = default)
    {
        if (!context.Users.Any())
        {
            var groupName = context.Notification.TenantId?.ToString() ?? "Global";
            try
            {
                var singalRGroup = _hubContext.Clients.Group(groupName);
                // 租户通知群发
                Logger.LogDebug($"Found a singalr group, begin senging notifications");
                await singalRGroup.SendAsync(_options.MethodName, context.Notification, cancellationToken);

                Logger.LogDebug("The notification: {0} with provider: {1} has successfully published!", context.Notification.Name, Name);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not send notifications to group {0}", groupName);

                context.Cancel(string.Format("Send to user notifications error: {0}", ex.Message), ex);
                Logger.LogWarning(context.Reason);
            }
        }
        else
        {
            try
            {
                var onlineClients = _hubContext.Clients.Users(context.Users.Select(x => x.UserId.ToString()));
                Logger.LogDebug($"Found a singalr client, begin senging notifications");
                await onlineClients.SendAsync(_options.MethodName, context.Notification, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not send notifications to all users");

                context.Cancel(string.Format("Send to user notifications error: {0}", ex.Message), ex);
                Logger.LogWarning(context.Reason);
            }
        }
    }
}
