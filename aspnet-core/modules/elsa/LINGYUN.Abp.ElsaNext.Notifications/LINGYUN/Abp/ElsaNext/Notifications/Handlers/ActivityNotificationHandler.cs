using Elsa.Mediator.Contracts;
using Elsa.Workflows.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.Notifications.Handlers;

public class ActivityNotificationHandler :
    INotificationHandler<ActivityExecuting>,
    INotificationHandler<ActivityExecuted>,
    INotificationHandler<ActivityCancelled>,
    INotificationHandler<ActivityCompleted>
{
    public ILogger<ActivityNotificationHandler> Logger { protected get; set; }
    public ActivityNotificationHandler()
    {
        Logger = NullLogger<ActivityNotificationHandler>.Instance;
    }

    public async virtual Task HandleAsync(ActivityExecuting notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The activity notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(ActivityExecuted notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The activity notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(ActivityCancelled notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The activity notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(ActivityCompleted notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The activity notification: {notification} has been triggered.", notification.ToString());
    }
}
