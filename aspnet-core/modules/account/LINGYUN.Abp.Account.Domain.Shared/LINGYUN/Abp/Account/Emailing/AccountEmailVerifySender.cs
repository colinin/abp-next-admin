using LINGYUN.Abp.Account.Localization;
using LY.MicroService.IdentityServer.Emailing.Templates;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Account.Emailing;

public class AccountEmailVerifySender : IAccountEmailVerifySender, ITransientDependency
{
    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }

    protected IStringLocalizer<AbpAccountResource> StringLocalizer { get; }

    public AccountEmailVerifySender(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizer<AbpAccountResource> stringLocalizer)
    {
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;
        StringLocalizer = stringLocalizer;
    }

    public async virtual Task SendMailLoginVerifyCodeAsync(
        string code,
        string userName,
        string emailAddress)
    {
        var emailContent = await TemplateRenderer.RenderAsync(
            AccountEmailTemplates.MailSecurityVerifyLink,
            new { code = code, user = userName }
        );

        await EmailSender.SendAsync(
            emailAddress,
            StringLocalizer["MailSecurityVerify"],
            emailContent
        );
    }
}
