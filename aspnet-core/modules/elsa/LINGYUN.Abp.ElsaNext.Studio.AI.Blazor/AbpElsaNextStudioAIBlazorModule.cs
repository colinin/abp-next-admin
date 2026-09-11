using Elsa.Studio.AI.Extensions;
using LINGYUN.Abp.ElsaNext.AI;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.AI.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.AI.Blazor;

[DependsOn(
    typeof(AbpElsaNextAIModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioAIBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpElsaNextStudioAIBlazorModule).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioAIMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioAIBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.AI.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/Localization/Resources/AIBlazor");
        });
        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddWeaverModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
    }
}
