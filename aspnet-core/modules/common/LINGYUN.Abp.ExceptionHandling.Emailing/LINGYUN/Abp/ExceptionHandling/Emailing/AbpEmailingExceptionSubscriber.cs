using LINGYUN.Abp.ExceptionHandling.Emailing.Localization;
using LINGYUN.Abp.ExceptionHandling.Emailing.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    public class AbpEmailingExceptionSubscriber : AbpExceptionSubscriberBase
    {
        protected IEmailSender EmailSender { get; }
        protected IStringLocalizer StringLocalizer { get; }
        protected ITemplateRenderer TemplateRenderer { get; }
        protected AbpEmailExceptionHandlingOptions EmailOptions { get; }
        public AbpEmailingExceptionSubscriber(
            IEmailSender emailSender,
            ITemplateRenderer templateRenderer,
            IServiceScopeFactory serviceScopeFactory, 
            IOptions<AbpExceptionHandlingOptions> options,
            IOptions<AbpEmailExceptionHandlingOptions> emailOptions,
            IStringLocalizer<ExceptionHandlingResource> stringLocalizer) 
            : base(serviceScopeFactory, options)
        {
            EmailSender = emailSender;
            EmailOptions = emailOptions.Value;
            StringLocalizer = stringLocalizer;
            TemplateRenderer = templateRenderer;
        }

        protected override async Task SendErrorNotifierAsync(ExceptionSendNotifierContext context)
        {
            // 需不需要用 SettingProvider 来获取?
            var receivedUsers = EmailOptions.GetReceivedEmailOrDefault(context.Exception.GetType());

            if (!receivedUsers.IsNullOrWhiteSpace())
            {
                var emailTitle = EmailOptions.DefaultTitle ?? L("SendEmailTitle");
                var templateContent = await TemplateRenderer
                    .RenderAsync(ExceptionHandlingTemplates.SendEmail,
                        new
                        {
                            title = emailTitle,
                            header = EmailOptions.DefaultContentHeader ?? L("SendEmailHeader"),
                            type = context.Exception.GetType().FullName,
                            message = context.Exception.Message,
                            loglevel = context.LogLevel.ToString(),
                            triggertime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            sendstacktrace = EmailOptions.SendStackTrace,
                            stacktrace = context.Exception.ToString(),
                            footer = EmailOptions.DefaultContentFooter ?? $"Copyright to LY Colin © {DateTime.Now.Year}"
                        });

                await EmailSender.SendAsync(receivedUsers,
                    emailTitle,
                    templateContent);
            }
        }

        protected string L(string name, params object[] args)
        {
            return StringLocalizer[name, args].Value;
        }
    }
}
