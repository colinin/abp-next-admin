using LINGYUN.Abp.Identity;
using LY.MicroService.IdentityServer.Emailing.Templates;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Sms;
using Volo.Abp.TextTemplating;

namespace LY.MicroService.IdentityServer;

public class UserSecurityCodeSender : IUserSecurityCodeSender, ITransientDependency
{
    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }
    protected IStringLocalizer<IdentityResource> Localizer { get; }

    protected ISmsSender SmsSender { get; }

    public UserSecurityCodeSender(
        ISmsSender smsSender,
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizer<IdentityResource> localizer)
    {
        SmsSender = smsSender;
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;
        Localizer = localizer;
    }

    public virtual async Task SendEmailConfirmedCodeAsync(
        string userName,
        string email,
        string token,
        CancellationToken cancellation = default)
    {
        var emailContent = await TemplateRenderer.RenderAsync(
            IdentityEmailTemplates.EmailConfirmed,
            new { user = userName, code = token });

        await EmailSender.SendAsync(
            email,
            Localizer["EmailConfirmed"],
            emailContent);
    }

    public virtual async Task SendPhoneConfirmedCodeAsync(
        string phone,
        string token,
        string template,
        CancellationToken cancellation = default)
    {
        Check.NotNullOrWhiteSpace(template, nameof(template));

        var smsMessage = new SmsMessage(phone, token);
        smsMessage.Properties.Add("code", token);
        smsMessage.Properties.Add("TemplateCode", template);

        await SmsSender.SendAsync(smsMessage);
    }
}
