using Volo.Abp.AspNetCore.Components.Web;

namespace LINGYUN.Abp.MicroService.AllInOne;

public partial class AllInOneModule
{
    private void PreConfigureBlazorUI()
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });
    }

    private void ConfigureBlazorUI(IServiceCollection services)
    {
        services.AddTransient<StudioBackendCookieHandler>();
        services.AddTransient<Elsa.Studio.Authentication.Abstractions.Contracts.IHttpConnectionOptionsConfigurator, StudioHttpConnectionOptionsConfigurator>();
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }
}
