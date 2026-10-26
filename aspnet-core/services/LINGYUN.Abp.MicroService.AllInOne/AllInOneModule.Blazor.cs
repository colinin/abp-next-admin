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

        PreConfigure<AbpElsaNextStudioBlazorOptions>(options =>
        {
            options.BackendApiConfig.ConfigureHttpClientBuilder = builder =>
            {
                builder.AuthenticationHandler = typeof(StudioBackendCookieHandler);
            };
        });
    }

    private void ConfigureBlazorUI(IServiceCollection services)
    {
        services.AddTransient<StudioBackendCookieHandler>();
        services.AddTransient<IHttpConnectionOptionsConfigurator, StudioHttpConnectionOptionsConfigurator>();
        services.AddRazorComponents().AddInteractiveServerComponents();
    }
}
