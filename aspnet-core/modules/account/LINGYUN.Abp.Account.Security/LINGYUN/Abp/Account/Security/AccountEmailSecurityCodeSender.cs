using LINGYUN.Abp.Account.Security.Localization;
using LINGYUN.Abp.Account.Security.Templates;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;

namespace LINGYUN.Abp.Account.Security;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IAccountEmailSecurityCodeSender),
    typeof(AccountEmailSecurityCodeSender))]
public class AccountEmailSecurityCodeSender : 
    IAccountEmailSecurityCodeSender,
    ITransientDependency
{
    protected ITemplateRenderer TemplateRenderer { get; }
    protected IEmailSender EmailSender { get; }
    protected IStringLocalizer<AccountSecurityResource> StringLocalizer { get; }
    protected IAppUrlProvider AppUrlProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AccountEmailSecurityCodeSender(
        IEmailSender emailSender,
        ICurrentTenant currentTenant,
        IAppUrlProvider appUrlProvider,
        ITemplateRenderer templateRenderer,
        IStringLocalizer<AccountSecurityResource> accountLocalizer)
    {
        EmailSender = emailSender;
        CurrentTenant = currentTenant;
        AppUrlProvider = appUrlProvider;
        StringLocalizer = accountLocalizer;
        TemplateRenderer = templateRenderer;
    }

    public async virtual Task SendLoginCodeAsync(
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

    public async virtual Task SendConfirmLinkAsync(
        Guid userId,
        string userEmail,
        string confirmToken,
        string appName,
        string returnUrl = null, 
        string returnUrlHash = null,
        Guid? userTenantId = null)
    {
        Debug.Assert(CurrentTenant.Id == userTenantId, "This method can only work for current tenant!");

        var url = await AppUrlProvider.GetUrlAsync(appName, AccountUrlNames.EmailConfirm);

        var link = $"{url}?userId={userId}&{TenantResolverConsts.DefaultTenantKey}={userTenantId}&confirmToken={UrlEncoder.Default.Encode(confirmToken)}";

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
            userEmail,
            StringLocalizer["EmailConfirm"],
            emailContent
        );
    }

    protected virtual string NormalizeReturnUrl(string returnUrl)
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

        if (returnUrl.StartsWith("/connect/authorize?", StringComparison.OrdinalIgnoreCase))
        {
            return HttpUtility.UrlEncode(returnUrl);
        }

        return returnUrl;
    }
}
