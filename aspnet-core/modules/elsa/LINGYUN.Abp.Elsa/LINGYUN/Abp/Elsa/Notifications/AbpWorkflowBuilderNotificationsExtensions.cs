using Elsa.Builders;

namespace LINGYUN.Abp.Elsa.Notifications;
public static class AbpWorkflowBuilderNotificationsExtensions
{
    internal const string NotificationPrefix = "_Abp_Elsa_Notification_";
    public const string FaultedNotificationKey = NotificationPrefix + "Faulted_";
    public const string CompletedNotificationKey = NotificationPrefix + "Completed_";
    public const string CancelledNotificationKey = NotificationPrefix + "Cancelled_";
    public const string SuspendedNotificationKey = NotificationPrefix + "Suspended_";

    public static IWorkflowBuilder WithFaultedNotification(this IWorkflowBuilder builder, string notification)
    {
        return builder.WithCustomAttribute(FaultedNotificationKey, notification);
    }

    public static IWorkflowBuilder WithCompletedNotification(this IWorkflowBuilder builder, string notification)
    {
        return builder.WithCustomAttribute(CompletedNotificationKey, notification);
    }

    public static IWorkflowBuilder WithCancelledNotification(this IWorkflowBuilder builder, string notification)
    {
        return builder.WithCustomAttribute(CancelledNotificationKey, notification);
    }

    public static IWorkflowBuilder WithSuspendedNotification(this IWorkflowBuilder builder, string notification)
    {
        return builder.WithCustomAttribute(SuspendedNotificationKey, notification);
    }
}
