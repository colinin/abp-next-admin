using Elsa.Studio.UserTasks.Extensions;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.UserTasks.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.UserTasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.UserTasks.Blazor;

[DependsOn(
    typeof(AbpElsaNextUserTasksModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioUserTasksBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.UserTasks.Feature).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioUserTasksMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioUserTasksBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.UserTasks.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/Localization/Resources/UserTasksBlazor");
        });
        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddUserTasksModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
    }
}
