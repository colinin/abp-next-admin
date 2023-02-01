using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services.Models;
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.Activities.Notifications;

[Action(
    Category = "Notification", 
    Description = "Send an notification.",
    Outcomes = new[] { OutcomeNames.Done })]
public class SendNotification : AbpActivity
{
    private readonly INotificationSender _notificationSender;

    [ActivityInput(Hint = "The name of the registered notification.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string NotificationName { get; set; }

    [ActivityInput(
            Hint = "Notifications pass data or template content.",
            UIHint = ActivityInputUIHints.MultiLine,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid },
            DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName
        )]
    public object NotificationData { get; set; }

    [ActivityInput(
        Hint = "The recipients user id list.", 
        UIHint = ActivityInputUIHints.MultiText, 
        DefaultSyntax = SyntaxNames.Json, 
        SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript })]
    public ICollection<Guid> To { get; set; } = new List<Guid>();

    [ActivityInput(Hint = "Notifications severity.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;

    [ActivityOutput]
    public string? NotificationId { get; set; }

    public SendNotification(
        INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }


    protected async override ValueTask<IActivityExecutionResult> OnActivityExecuteAsync(ActivityExecutionContext context)
    {
        switch (NotificationData)
        {
            case NotificationData data:
                NotificationId = await _notificationSender.SendNofiterAsync(
                    NotificationName,
                    data,
                    GetUserIdentifiers(),
                    TenantId,
                    Severity);
                return Done();
            case NotificationTemplate template:
                NotificationId = await _notificationSender.SendNofiterAsync(
                    NotificationName,
                    template,
                    GetUserIdentifiers(),
                    TenantId,
                    Severity);
                return Done();
        }

        return Suspend();
    }

    private IEnumerable<UserIdentifier> GetUserIdentifiers()
    {
        return To.Select(to => new UserIdentifier(to, to.ToString()));
    }
}
