using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Elsa.Activities.Emailing;

/// <summary>
/// 发送邮件
/// </summary>
/// <remarks>
/// 与elsa基础SendEmail命名冲突,故改名
/// 此活动使用abp框架内置邮件发送接口<see cref="IEmailSender"/>
/// </remarks>
[Action(
    Category = "Emailing", 
    Description = "Send an email message.",
    Outcomes = new[] { OutcomeNames.Done })]
public class SendEmailing : AbpActivity
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateRenderer _templateRenderer;

    public SendEmailing(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer)
    {
        _emailSender = emailSender;
        _templateRenderer = templateRenderer;
    }

    [ActivityInput(Hint = "The recipients email addresses.", UIHint = ActivityInputUIHints.MultiText, DefaultSyntax = SyntaxNames.Json, SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript })]
    public ICollection<string> To { get; set; } = new List<string>();

    [ActivityInput(Hint = "The subject of the email message.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string? Subject { get; set; }

    [ActivityInput(Hint = "The body of the email message.", UIHint = ActivityInputUIHints.MultiLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string? Body { get; set; }

    [ActivityInput(Hint = "The body of the email message.", UIHint = ActivityInputUIHints.MultiLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string? Culture { get; set; }

    [ActivityInput(Hint = "The template name of render the email message.", UIHint = ActivityInputUIHints.MultiLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string? Template { get; set; }

    [ActivityInput(
            Hint = "Model parameters used to format the contents of the template.",
            UIHint = ActivityInputUIHints.MultiLine,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid },
            DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName
        )]
    public object? Model { get; set; }

    protected async override ValueTask<IActivityExecutionResult> OnActivitExecuteAsync(ActivityExecutionContext context)
    {
        var to = To.JoinAsString(";");
        var content = Body;

        if (!Template.IsNullOrWhiteSpace())
        {
            content = await _templateRenderer.RenderAsync(
                Template,
                Model,
                Culture);
        }

        await _emailSender.SendAsync(to, Subject, content);

        return Done();
    }
}
