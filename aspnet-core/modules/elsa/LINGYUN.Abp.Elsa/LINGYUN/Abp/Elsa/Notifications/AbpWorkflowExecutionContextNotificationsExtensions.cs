using Elsa.Services.Models;
using System.Collections.Generic;

namespace LINGYUN.Abp.Elsa.Notifications;
public static class AbpWorkflowExecutionContextNotificationsExtensions
{
    private const string NotificationDataKey = AbpWorkflowBuilderNotificationsExtensions.NotificationPrefix + "Data_";

    public static void WithNotificationData(this ActivityExecutionContext context, string key, object data)
    {
        context.WorkflowExecutionContext.SetTransientVariable(key, data);
    }

    public static void WithNotificationData(this WorkflowExecutionContext context, string key, object data)
    {
        var notificationData = context.GetNotificationData();

        notificationData.TryAdd(key, data);

        context.SetTransientVariable(NotificationDataKey, notificationData);
    }

    public static Dictionary<string, object> GetNotificationData(this ActivityExecutionContext context)
    {
        return context.WorkflowExecutionContext.GetNotificationData();
    }

    public static Dictionary<string, object> GetNotificationData(this WorkflowExecutionContext context)
    {
        var notificationData = context.GetTransientVariable<Dictionary<string, object>>(NotificationDataKey);
        notificationData ??= new Dictionary<string, object>();

        return notificationData;
    }

    public static string? GetFaultedNotification(this WorkflowExecutionContext context)
    {
        return context.WorkflowBlueprint.CustomAttributes
            .Get<string>(AbpWorkflowBuilderNotificationsExtensions.FaultedNotificationKey);
    }

    public static string? GetCompletedNotification(this WorkflowExecutionContext context)
    {
        return context.WorkflowBlueprint.CustomAttributes
            .Get<string>(AbpWorkflowBuilderNotificationsExtensions.CompletedNotificationKey);
    }

    public static string? GetSuspendedNotification(this WorkflowExecutionContext context)
    {
        return context.WorkflowBlueprint.CustomAttributes
            .Get<string>(AbpWorkflowBuilderNotificationsExtensions.SuspendedNotificationKey);
    }

    public static string? GetCancelledNotification(this WorkflowExecutionContext context)
    {
        return context.WorkflowBlueprint.CustomAttributes
            .Get<string>(AbpWorkflowBuilderNotificationsExtensions.CancelledNotificationKey);
    }
}
