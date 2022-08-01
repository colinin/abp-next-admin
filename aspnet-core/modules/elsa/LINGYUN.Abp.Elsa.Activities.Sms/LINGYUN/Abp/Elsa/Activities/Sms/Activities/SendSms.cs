using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Elsa.Activities.Sms;

[Action(
    Category = "Sms", 
    Description = "Send an sms message.",
    Outcomes = new[] { OutcomeNames.Done })]
public class SendSms : Activity
{
    private readonly ISmsSender _smsSender;

    public SendSms(
        ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    [ActivityInput(
        Hint = "The recipients phoneNumber list.",
        UIHint = ActivityInputUIHints.MultiText,
        DefaultSyntax = SyntaxNames.Json,
        SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript })]
    public ICollection<string> To { get; set; } = new List<string>();

    [ActivityInput(Hint = "The message content.")]
    public string Message { get; set; }

    [ActivityInput(
           Hint = "Attachment property that are sent with the message.",
           UIHint = ActivityInputUIHints.MultiLine, DefaultSyntax = SyntaxNames.Json,
           SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid },
           Category = PropertyCategories.Advanced
       )]
    public Dictionary<string, string> Properties { get; set; } = new();

    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        var smsMessage = new SmsMessage(To.JoinAsString(";"), Message);
        foreach (var prop in Properties)
        {
            smsMessage.Properties.TryAdd(prop.Key, prop.Value);
        }

        await _smsSender.SendAsync(smsMessage);

        return Done();
    }
}
