using Elsa.Studio.Secrets.Extensions;
using LINGYUN.Abp.ElsaNext.Secrets.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Secrets.Blazor;

[DependsOn(
    typeof(AbpElsaNextSecretsModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextSecretsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Secrets.Feature).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioSecretsMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextSecretsBlazorModule>("LINGYUN.Abp.ElsaNext.Secrets.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaStudioResource>()
                .AddVirtualJson("/Localization/Resources/SecretsBlazor");
        });

        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddSecretsModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
    }
}
