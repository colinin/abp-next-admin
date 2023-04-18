using Elsa.Events;
using Elsa.Models;
using Elsa.Services.Models;
using LINGYUN.Abp.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Elsa.Notifications;

public class AbpElsaWorkflowNotificationHandler :
    INotificationHandler<WorkflowFaulted>,
    INotificationHandler<WorkflowCancelled>,
    INotificationHandler<WorkflowCompleted>,
    INotificationHandler<WorkflowSuspended>
{
    public async Task Handle(WorkflowFaulted notification, CancellationToken cancellationToken)
    {
        await SendNotifierIfExistsAsync(
            notification.WorkflowExecutionContext.GetFaultedNotification(),
            notification.WorkflowExecutionContext,
            severity: NotificationSeverity.Error);
    }

    public async Task Handle(WorkflowCancelled notification, CancellationToken cancellationToken)
    {
        await SendNotifierIfExistsAsync(
            notification.WorkflowExecutionContext.GetCancelledNotification(),
            notification.WorkflowExecutionContext,
            severity: NotificationSeverity.Warn);
    }

    public async Task Handle(WorkflowCompleted notification, CancellationToken cancellationToken)
    {
        await SendNotifierIfExistsAsync(
            notification.WorkflowExecutionContext.GetCompletedNotification(),
             notification.WorkflowExecutionContext,
             severity: NotificationSeverity.Success);
    }

    public async Task Handle(WorkflowSuspended notification, CancellationToken cancellationToken)
    {
        await SendNotifierIfExistsAsync(
            notification.WorkflowExecutionContext.GetSuspendedNotification(),
            notification.WorkflowExecutionContext,
            severity: NotificationSeverity.Warn);
    }

    private async Task SendNotifierIfExistsAsync(
        string notificationName,
        WorkflowExecutionContext executionContext,
        NotificationSeverity severity = NotificationSeverity.Info,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (notificationName.IsNullOrWhiteSpace())
            {
                return;
            }

            var workflowInstance = executionContext.WorkflowInstance;
            var workflowBlueprint = executionContext.WorkflowBlueprint;

            var serviceProvider = executionContext.ServiceProvider;
            var currentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();
            var notificationSender = serviceProvider.GetRequiredService<INotificationSender>();

            var notificationData = new Dictionary<string, object>
            {
                { nameof(IActivityBlueprint.Id),  workflowBlueprint.Id },
                { nameof(WorkflowExecutionContext.Status), executionContext.Status },
            };
            if (!workflowBlueprint.Name.IsNullOrWhiteSpace())
            {
                notificationData.TryAdd(nameof(IActivityBlueprint.Name), workflowBlueprint.Name);
            }
            if (!workflowBlueprint.DisplayName.IsNullOrWhiteSpace())
            {
                notificationData.TryAdd(nameof(IActivityBlueprint.DisplayName), workflowBlueprint.DisplayName);
            }
            if (!workflowBlueprint.Description.IsNullOrWhiteSpace())
            {
                notificationData.TryAdd(nameof(IActivityBlueprint.Description), workflowBlueprint.Description);
            }
            if (!workflowBlueprint.Type.IsNullOrWhiteSpace())
            {
                notificationData.TryAdd(nameof(IActivityBlueprint.Type), workflowBlueprint.Type);
            }
            if (executionContext.Exception != null)
            {
                notificationData.TryAdd(nameof(Exception), executionContext.Exception.Message);
            }

            var transientData = executionContext.GetNotificationData();
            foreach (var data in transientData)
            {
                notificationData.TryAdd(data.Key, data.Value);
            }

            var notificationTemplate = new NotificationTemplate(
                notificationName,
                CultureInfo.CurrentCulture.Name,
                data: notificationData);

            await notificationSender.SendNofiterAsync(
                notificationName,
                notificationTemplate,
                tenantId: currentTenant.Id ?? workflowInstance.GetTenantId(),
                severity: severity);
        }
        catch (Exception ex)
        {
            executionContext
                .ServiceProvider
                .GetService<ILogger<AbpElsaWorkflowNotificationHandler>>()
                ?.LogWarning("Failed to send a workflow notification, because: {Message}", ex.Message);
        }
    }
}
