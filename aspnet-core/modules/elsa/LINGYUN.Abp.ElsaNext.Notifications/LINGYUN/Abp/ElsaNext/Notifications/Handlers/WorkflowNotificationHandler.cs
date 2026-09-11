using Elsa.Mediator.Contracts;
using Elsa.Workflows.Notifications;
using Elsa.Workflows.Runtime.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.Notifications.Handlers;

public class WorkflowNotificationHandler :
    INotificationHandler<WorkflowExecuting>,
    INotificationHandler<WorkflowStarted>,
    INotificationHandler<WorkflowFinished>,
    INotificationHandler<WorkflowExecuted>,
    INotificationHandler<WorkflowCancelling>,
    INotificationHandler<WorkflowCancelled>
{
    public ILogger<WorkflowNotificationHandler> Logger { protected get; set; }
    public WorkflowNotificationHandler()
    {
        Logger = NullLogger<WorkflowNotificationHandler>.Instance;
    }
    public async virtual Task HandleAsync(WorkflowExecuting notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The workflow notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(WorkflowStarted notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The workflow notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(WorkflowFinished notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The workflow notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(WorkflowExecuted notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The workflow notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(WorkflowCancelling notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The workflow notification: {notification} has been triggered.", notification.ToString());
    }

    public async virtual Task HandleAsync(WorkflowCancelled notification, CancellationToken cancellationToken)
    {
        Logger.LogDebug("The workflow notification: {notification} has been triggered.", notification.ToString());
    }
}
