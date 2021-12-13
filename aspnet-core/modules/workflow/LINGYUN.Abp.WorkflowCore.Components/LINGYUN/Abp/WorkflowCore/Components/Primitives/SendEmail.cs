using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.TextTemplating;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Components.Primitives
{
    public class SendEmail : StepBodyAsyncBase
    {
        public ILogger<SendEmail> Logger { protected get; set; }

        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly IBlobContainer<WorkflowContainer> _blobContainer;

        public SendEmail(
            IEmailSender emailSender,
            ITemplateRenderer templateRenderer,
            IBlobContainer<WorkflowContainer> blobContainer)
        {
            _emailSender = emailSender;
            _blobContainer = blobContainer;
            _templateRenderer = templateRenderer;

            Logger = NullLogger<SendEmail>.Instance;
        }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Receivers { get; set; }

        [CanBeNull]
        public object Data { get; set; }

        [CanBeNull]
        public string Template { get; set; }

        [CanBeNull]
        public string Attachments { get; set; }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            Logger.LogInformation("Working on sending email step.");

            var templateContent = await _templateRenderer.RenderAsync(
                Template.IsNullOrWhiteSpace() ? StandardEmailTemplates.Message : Template,
                Data,
                CultureInfo.CurrentCulture.Name);

            var mailMessage = new MailMessage
            {
                To = { Receivers },
                Subject = Title,
                Body = templateContent,
                IsBodyHtml = true
            };

            if (!Attachments.IsNullOrWhiteSpace())
            {
                var attachments = Attachments.Split(';');
                foreach (var attachment in attachments)
                {
                    var stream = await _blobContainer.GetOrNullAsync(attachment);
                    if (stream != null && stream != Stream.Null)
                    {
                        mailMessage.Attachments.Add(new Attachment(stream, Path.GetFileName(attachment)));
                    }
                }
            }

            await _emailSender.SendAsync(mailMessage);

            Logger.LogInformation("Email sent, forward to next step.");

            return ExecutionResult.Next();
        }
    }
}
