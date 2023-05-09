using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Elsa.Notifications;
public static class AbpWorkflowExecutionContextNotificationsExtensions
{
    private const string NotificationDataKey = AbpWorkflowBuilderNotificationsExtensions.NotificationPrefix + "Data_";

    #region MultiTenancy

    public static void WithNotificationTenantId(this ActivityExecutionContext context, Guid? tenantId)
    {
        context.WorkflowExecutionContext.WithNotificationTenantId(tenantId);
    }

    public static void WithNotificationTenantId(this WorkflowExecutionContext context, Guid? tenantId)
    {
        if (tenantId.HasValue)
        {
            context.WithNotificationData(nameof(IMultiTenant.TenantId), tenantId.Value);
        }
    }

    public static Guid? GetNotificationTenantId(this ActivityExecutionContext context)
    {
        return context.WorkflowExecutionContext.GetNotificationTenantId();
    }

    public static Guid? GetNotificationTenantId(this WorkflowExecutionContext context)
    {
        var data = context.GetNotificationData();
        if (data.TryGetValue(nameof(IMultiTenant.TenantId), out var tenantIdObj) &&
            tenantIdObj != null && Guid.TryParse(tenantIdObj.ToString(), out Guid tenantId))
        {
            return tenantId;
        }

        return null;
    }

    #endregion

    public static void WithNotificationData(this ActivityExecutionContext context, string key, object data)
    {
        context.WorkflowExecutionContext.WithNotificationData(key, data);
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
