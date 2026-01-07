using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LINGYUN.Abp.MicroService.AuthServer.Ui.Branding;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBrandingProvider), typeof(AccountBrandingProvider))]
public class AccountBrandingProvider : IBrandingProvider, ITransientDependency
{
    private readonly AccountBrandingOptions _options;

    public AccountBrandingProvider(IOptions<AccountBrandingOptions> options)
    {
        _options = options.Value;
    }

    public string AppName => _options.AppName ?? "MyApplication";

    public string LogoUrl => _options.LogoUrl;

    public string LogoReverseUrl => _options.LogoReverseUrl;
}
