using Elsa.Studio.Alterations.Extensions;
using LINGYUN.Abp.ElsaNext.Alterations;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Alterations.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Alterations.Blazor;

[DependsOn(
    typeof(AbpElsaNextAlterationsModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioAlterationsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Alterations.Feature).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioAlterationsMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioAlterationsBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.Alterations.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/Localization/Resources/AlterationsBlazor");
        });

        context.Services.AddAlterationsModule();
    }
}
