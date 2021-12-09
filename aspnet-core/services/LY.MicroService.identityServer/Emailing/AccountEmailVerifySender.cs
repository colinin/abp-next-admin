using LY.MicroService.IdentityServer.Emailing.Templates;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;

namespace LY.MicroService.IdentityServer.Emailing;

[Dependency(ReplaceServices = true)]
[ExposeServices(
        typeof(IAccountEmailer),
        typeof(AccountEmailer),
        typeof(IAccountEmailVerifySender),
        typeof(AccountEmailVerifySender))]
public class AccountEmailVerifySender : AccountEmailer, IAccountEmailVerifySender, ITransientDependency
{
    public AccountEmailVerifySender(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizer<AccountResource> stringLocalizer,
        IAppUrlProvider appUrlProvider,
        ICurrentTenant currentTenant)
        : base(emailSender, templateRenderer, stringLocalizer, appUrlProvider, currentTenant)
    {
    }

    public virtual async Task SendMailLoginVerifyLinkAsync(
        IdentityUser user,
        string code,
        string appName,
        string provider,
        bool rememberMe = false,
        string returnUrl = null,
        string returnUrlHash = null)
    {
        Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

        // TODO: 需要生成快捷链接
        //var url = await AppUrlProvider.GetUrlAsync(appName, AccountUrlNames.MailLoginVerify);

        //var link = $"{url}?provider={provider}&rememberMe={rememberMe}&resetToken={UrlEncoder.Default.Encode(code)}";

        //if (!returnUrl.IsNullOrEmpty())
        //{
        //    link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
        //}

        //if (!returnUrlHash.IsNullOrEmpty())
        //{
        //    link += "&returnUrlHash=" + returnUrlHash;
        //}

        var emailContent = await TemplateRenderer.RenderAsync(
            AccountEmailTemplates.MailSecurityVerifyLink,
            new { code = code, user = user.UserName }
        );

        await EmailSender.SendAsync(
            user.Email,
            StringLocalizer["MailSecurityVerify"],
            emailContent
        );
    }

    protected override string NormalizeReturnUrl(string returnUrl)
    {
        if (returnUrl.IsNullOrEmpty())
        {
            return returnUrl;
        }

        //Handling openid connect login
        if (returnUrl.StartsWith("/connect/authorize/callback", StringComparison.OrdinalIgnoreCase))
        {
            if (returnUrl.Contains("?"))
            {
                var queryPart = returnUrl.Split('?')[1];
                var queryParameters = queryPart.Split('&');
                foreach (var queryParameter in queryParameters)
                {
                    if (queryParameter.Contains("="))
                    {
                        var queryParam = queryParameter.Split('=');
                        if (queryParam[0] == "redirect_uri")
                        {
                            return HttpUtility.UrlDecode(queryParam[1]);
                        }
                    }
                }
            }
        }

        return returnUrl;
    }
}
