using Elsa.Studio.Extensions;
using LINGYUN.Abp.ElsaNext.Agents.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Agents.Blazor;

[DependsOn(
    typeof(AbpElsaNextAgentsModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextAgentsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Agents.Feature).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioAgentsMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextAgentsBlazorModule>("LINGYUN.Abp.ElsaNext.Agents.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaStudioResource>()
                .AddVirtualJson("/Localization/Resources/AgentsBlazor");
        });

        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddAgentsModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
    }
}
