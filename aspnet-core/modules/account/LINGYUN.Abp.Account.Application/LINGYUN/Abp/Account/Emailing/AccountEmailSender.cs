using LINGYUN.Abp.Account.Emailing.Templates;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;

namespace LINGYUN.Abp.Account.Emailing;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IAccountEmailConfirmSender),
    typeof(IAccountEmailVerifySender),
    typeof(IAccountEmailer),
    typeof(AccountEmailSender))]
public class AccountEmailSender : 
    AccountEmailer, 
    IAccountEmailConfirmSender,
    IAccountEmailVerifySender,
    ITransientDependency
{
    public AccountEmailSender(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizer<AccountResource> accountLocalizer,
        IAppUrlProvider appUrlProvider,
        ICurrentTenant currentTenant)
        : base(emailSender, templateRenderer, accountLocalizer, appUrlProvider, currentTenant)
    {
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

    public async virtual Task SendEmailConfirmLinkAsync(
        IdentityUser user,
        string confirmToken,
        string appName,
        string returnUrl = null, 
        string returnUrlHash = null)
    {
        Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

        var url = await AppUrlProvider.GetUrlAsync(appName, AccountUrlNames.EmailConfirm);

        var link = $"{url}?userId={user.Id}&{TenantResolverConsts.DefaultTenantKey}={user.TenantId}&confirmToken={UrlEncoder.Default.Encode(confirmToken)}";

        if (!returnUrl.IsNullOrEmpty())
        {
            link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
        }

        if (!returnUrlHash.IsNullOrEmpty())
        {
            link += "&returnUrlHash=" + returnUrlHash;
        }

        var emailContent = await TemplateRenderer.RenderAsync(
            AccountEmailTemplates.MailConfirmLink,
            new { link = link }
        );

        await EmailSender.SendAsync(
            user.Email,
            StringLocalizer["EmailConfirm"],
            emailContent
        );
    }
}
