using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Emailing;

namespace LINGYUN.Abp.ExceptionHandling
{
    public class AbpEmailingExceptionSubscriber : AbpExceptionSubscriberBase
    {
        protected IEmailSender EmailSender { get; }
        protected AbpEmailExceptionHandlingOptions EmailOptions { get; }
        public AbpEmailingExceptionSubscriber(
            IEmailSender emailSender,
            IServiceScopeFactory serviceScopeFactory, 
            IOptions<AbpExceptionHandlingOptions> options,
            IOptions<AbpEmailExceptionHandlingOptions> emailOptions) 
            : base(serviceScopeFactory, options)
        {
            EmailSender = emailSender;
            EmailOptions = emailOptions.Value;
        }

        protected override async Task SendErrorNotifierAsync(ExceptionSendNotifierContext context)
        {
            var receivedUsers = EmailOptions.GetReceivedEmailOrDefault(context.Exception);

            if (!receivedUsers.IsNullOrWhiteSpace())
            {
                // TODO: 使用 Template 格式化推送
                await EmailSender.SendAsync(receivedUsers, 
                    context.Exception.GetType().FullName,
                    context.Exception.Message);
            }
        }
    }
}
