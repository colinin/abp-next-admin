using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders;

public abstract class OAuthHandlerOptionsProvider<TOptions> : IOAuthHandlerOptionsProvider<TOptions>, ITransientDependency
    where TOptions : RemoteAuthenticationOptions, new()
{
    protected ISettingProvider SettingProvider { get; }
    public OAuthHandlerOptionsProvider(ISettingProvider settingProvider)
    {
        SettingProvider = settingProvider;
    }

    public virtual Task SetOptionsAsync(TOptions options)
    {
        options.CorrelationCookie.SameSite = SameSiteMode.Lax;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.CorrelationCookie.HttpOnly = true;

        return Task.CompletedTask;
    }
}
