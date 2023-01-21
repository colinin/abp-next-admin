using Elsa.Builders;
using Elsa.Services.Models;
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.Notifications;
public static class SendNotificationExtensions
{
    public static ISetupActivity<SendNotification> WithNotificationName(
        this ISetupActivity<SendNotification> activity,
        Func<ActivityExecutionContext, ValueTask<string>> value) => activity.Set(x => x.NotificationName, value!);

    public static ISetupActivity<SendNotification> WithNotificationName(
        this ISetupActivity<SendNotification> activity,
        Func<ActivityExecutionContext, string> value) => activity.Set(x => x.NotificationName, value!);

    public static ISetupActivity<SendNotification> WithNotificationName(
        this ISetupActivity<SendNotification> activity,
        Func<string> value) => activity.Set(x => x.NotificationName, value!);

    public static ISetupActivity<SendNotification> WithNotificationName(
        this ISetupActivity<SendNotification> activity,
        string value) => activity.Set(x => x.NotificationName, value!);

    public static ISetupActivity<SendNotification> WithNotificationData(
       this ISetupActivity<SendNotification> activity,
       Func<ActivityExecutionContext, ValueTask<object>> value) => activity.Set(x => x.NotificationData, value!);

    public static ISetupActivity<SendNotification> WithNotificationData(
        this ISetupActivity<SendNotification> activity,
        Func<ActivityExecutionContext, object> value) => activity.Set(x => x.NotificationData, value!);

    public static ISetupActivity<SendNotification> WithNotificationData(
        this ISetupActivity<SendNotification> activity,
        Func<object> value) => activity.Set(x => x.NotificationData, value!);

    public static ISetupActivity<SendNotification> WithNotificationData(
        this ISetupActivity<SendNotification> activity,
        object value) => activity.Set(x => x.NotificationData, value!);

    public static ISetupActivity<SendNotification> WithSeverity(
       this ISetupActivity<SendNotification> activity,
       Func<ActivityExecutionContext, ValueTask<NotificationSeverity>> value) => activity.Set(x => x.Severity, value!);

    public static ISetupActivity<SendNotification> WithSeverity(
        this ISetupActivity<SendNotification> activity,
        Func<ActivityExecutionContext, NotificationSeverity> value) => activity.Set(x => x.Severity, value!);

    public static ISetupActivity<SendNotification> WithSeverity(
        this ISetupActivity<SendNotification> activity,
        Func<NotificationSeverity> value) => activity.Set(x => x.Severity, value!);

    public static ISetupActivity<SendNotification> WithSeverity(
        this ISetupActivity<SendNotification> activity,
        NotificationSeverity value) => activity.Set(x => x.Severity, value!);

    public static ISetupActivity<SendNotification> WithTo(
       this ISetupActivity<SendNotification> activity,
       Func<ActivityExecutionContext, ValueTask<ICollection<Guid>>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendNotification> WithTo(
        this ISetupActivity<SendNotification> activity,
        Func<ActivityExecutionContext, ICollection<Guid>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendNotification> WithTo(
        this ISetupActivity<SendNotification> activity,
        Func<ICollection<Guid>> value) => activity.Set(x => x.To, value!);

    public static ISetupActivity<SendNotification> WithTo(
        this ISetupActivity<SendNotification> activity,
        ICollection<Guid> value) => activity.Set(x => x.To, value!);
}
