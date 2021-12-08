using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.TextTemplating;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Components.Primitives
{
    public class EmailStepBody : StepBodyAsyncBase
    {
        public ILogger<EmailStepBody> Logger { protected get; set; }

        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;

        public EmailStepBody(
            IEmailSender emailSender,
            ITemplateRenderer templateRenderer)
        {
            _emailSender = emailSender;
            _templateRenderer = templateRenderer;

            Logger = NullLogger<EmailStepBody>.Instance;
        }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Receivers { get; set; }

        [CanBeNull]
        public object Data { get; set; }

        [CanBeNull]
        public string Template { get; set; }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            Logger.LogInformation("Working on sending email step.");

            var templateContent = await _templateRenderer.RenderAsync(
                Template.IsNullOrWhiteSpace() ? StandardEmailTemplates.Message : Template,
                Data,
                CultureInfo.CurrentCulture.Name);

            await _emailSender.SendAsync(Receivers, Title, templateContent);

            Logger.LogInformation("Email sent, forward to next step.");

            return ExecutionResult.Next();
        }
    }
}
