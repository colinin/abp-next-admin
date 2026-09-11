using Elsa.Studio.Labels;
using LINGYUN.Abp.ElsaNext.Labels;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Labels.Blazor.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Labels.Blazor;

[DependsOn(
    typeof(AbpElsaNextLabelsModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioLabelsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Labels.Feature).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioLabelsMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioLabelsBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.Labels.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/Localization/Resources/LabelsBlazor");
        });
        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddLabelsModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
    }
}
