using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Elsa.Workflows.UIHints;
using LINGYUN.Abp.ElsaNext.Notifications.UIHints;
using LINGYUN.Abp.ElsaNext.UIHints;
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using INotificationSender = LINGYUN.Abp.Notifications.INotificationSender;

namespace LINGYUN.Abp.ElsaNext.Notifications.Activities;

[Activity("Elsa", "Notifications", "Send an notification.", Kind = ActivityKind.Task)]
public class SendNotification : CodeActivity
{
    [Input(
        Description = "The name of the registered notification.",
        UIHint = NotificationsUIHints.NotificationPicker)]
    public Input<string> NotificationName { get; set; } = default!;

    [Input(
        Description = "The notification title.",
        UIHint = InputUIHints.MultiLine
    )]
    public Input<string?> Title { get; set; } = default!;

    [Input(
        Description = "The notification content.",
        UIHint = InputUIHints.MultiLine
    )]
    public Input<string?> Content { get; set; } = default!;

    [Input(
        Description = "The notification description.",
        UIHint = InputUIHints.MultiLine
    )]
    public Input<string?> Description { get; set; } = default!;

    [Input(
        Description = "The notification template name.",
        UIHint = InputUIHints.MultiLine
    )]
    public Input<string?> Template { get; set; } = default!;

    [Input(
        Description = "The notification additional attributes.",
        UIHint = InputUIHints.Dictionary
    )]
    public Input<Dictionary<string, object?>> Propertites { get; set; } = default!;

    [Input(
        Description = "The tenant id for sending the notification.",
        UIHint = AbpElsaUIHints.TenantPicker)]
    public Input<Guid?> TenantId { get; set; } = null!;

    [Input(
        Description = "The recipients email addresses.", 
        UIHint = AbpElsaUIHints.UserPickerMultiple)]
    public Input<ICollection<Guid>> To { get; set; } = null!;

    [Input(
        Description = "The notifications severity.",
        Options = new[] { "Success", "Info", "Warn", "Error", "Fatal" },
        DefaultValue = "Info",
        UIHint = InputUIHints.DropDown)]
    public Input<string?> Severity { get; set; } = null!;

    [Output(Description = "The notification id.")]
    public Output<string?> NotificationId { get; set; } = null!;

    protected async override ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var notificationSender = context.GetRequiredService<INotificationSender>();
        var currentTenant = context.GetRequiredService<ICurrentTenant>();
        var clock = context.GetRequiredService<IClock>();

        var tenantId = TenantId.GetOrDefault(context, () => currentTenant.Id);
        var userIdentifiers = GetUserIdentifiers(context);
        var severity = GetNotificationSeverity(context);
        var propertites = Propertites.GetOrDefault(context);
        var notificationName = NotificationName.Get(context);
        var template = Template.GetOrDefault(context);
        if (!string.IsNullOrWhiteSpace(template))
        {
            var notificationTemplate = new NotificationTemplate(template, data: propertites);
            var notificationTemplateId = await notificationSender.SendNofiterAsync(
                notificationName,
                notificationTemplate,
                userIdentifiers,
                tenantId,
                severity);
            NotificationId.Set(context, notificationTemplateId);
        }
        else
        {
            var title = Title.Get(context);
            var message = Content.Get(context);
            var description = Description.GetOrDefault(context) ?? "";

            var notificationData = new NotificationData();
            notificationData.WriteStandardData(title!, message!, clock.Now, "elsa-workflow", description);
            var notificationId = await notificationSender.SendNofiterAsync(
                notificationName,
                notificationData,
                userIdentifiers,
                tenantId,
                severity);
            NotificationId.Set(context, notificationId);
        }
    }

    private NotificationSeverity GetNotificationSeverity(ActivityExecutionContext context)
    {
        var severitySet = Severity.GetOrDefault(context);
        if (!string.IsNullOrWhiteSpace(severitySet))
        {
            if (Enum.TryParse<NotificationSeverity>(severitySet, true, out var severity))
            {
                return severity;
            }
        }
        return NotificationSeverity.Info;
    }

    private IEnumerable<UserIdentifier>? GetUserIdentifiers(ActivityExecutionContext context)
    {
        var userIdentifiers = To.GetOrDefault(context);
        return userIdentifiers?.Select(to => new UserIdentifier(to, to.ToString()));
    }
}
