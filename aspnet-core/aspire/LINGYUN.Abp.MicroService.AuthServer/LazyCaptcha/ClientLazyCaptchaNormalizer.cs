using LINGYUN.Abp.Captcha;
using LINGYUN.Abp.LazyCaptcha;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MicroService.AuthServer.LazyCaptcha;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ILazyCaptchaNormalizer),
    typeof(LazyCaptchaNormalizer))]
public class ClientLazyCaptchaNormalizer : LazyCaptchaNormalizer
{
    protected IWebClientInfoProvider WebClientInfoProvider { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected ICurrentUser CurrentUser { get; }
    public ClientLazyCaptchaNormalizer(
        IWebClientInfoProvider webClientInfoProvider,
        IHttpContextAccessor httpContextAccessor,
        ICurrentClient _currentClient,
        ICurrentUser _currentUser, 
        ICurrentTenant _currentTenant)
        : base(_currentClient, _currentUser, _currentTenant)
    {
        WebClientInfoProvider = webClientInfoProvider;
        HttpContextAccessor = httpContextAccessor;
        CurrentUser = _currentUser;
    }

    public override string NormalizeRateLimitKey(string captchaId)
    {
        var rateLimitKeyBuilder = new StringBuilder();
        if (!CurrentUser.IsAuthenticated &&
            HttpContextAccessor.HttpContext != null &&
            HttpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CaptchaKeywords.CaptchaIdCookieName, out var captchaKey) &&
            !captchaKey.IsNullOrWhiteSpace())
        {
            rateLimitKeyBuilder.Append("ck:");
            rateLimitKeyBuilder.Append(captchaKey);
            rateLimitKeyBuilder.Append(SeparatorChar);
        }
        if (!WebClientInfoProvider.ClientIpAddress.IsNullOrWhiteSpace())
        {
            rateLimitKeyBuilder.Append("ip:");
            rateLimitKeyBuilder.Append(WebClientInfoProvider.ClientIpAddress);
            rateLimitKeyBuilder.Append(SeparatorChar);
        }
        rateLimitKeyBuilder.Append(base.NormalizeRateLimitKey(captchaId));
        return rateLimitKeyBuilder.ToString();
    }
}
