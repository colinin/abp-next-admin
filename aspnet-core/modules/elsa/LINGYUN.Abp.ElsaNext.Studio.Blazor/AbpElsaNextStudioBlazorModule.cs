using Elsa.Studio.Core.BlazorServer.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Localization.BlazorServer.Extensions;
using Elsa.Studio.Localization.Models;
using Elsa.Studio.Models;
using Elsa.Studio.Settings.Extensions;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Designer.Options;
using Elsa.Studio.Workflows.Extensions;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor;

[DependsOn(
    typeof(AbpElsaNextModule),
    typeof(AbpElsaNextStudioTranslationsModule),
    typeof(AbpLocalizationModule),
    typeof(AbpUiNavigationModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule))]
public class AbpElsaNextStudioBlazorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        PreConfigure<AbpElsaNextStudioBlazorOptions>(options =>
        {
            options.BackendApiConfig = new BackendApiConfig
            {
                ConfigureBackendOptions = backend => configuration.GetSection("Backend").Bind(backend),
            };
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Settings.Feature).Assembly);
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Workflows.Feature).Assembly);
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaStudioResource>()
                .AddBaseTypes(typeof(ElsaNextResource))
                .AddVirtualJson("/Localization/Resources/ElsaStudioBlazor");
        });

        context.Services.AddCore(options => configuration.GetSection("Presentation").Bind(options));
        context.Services.AddShell(options => configuration.GetSection("Shell").Bind(options));

        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddRemoteBackend(elsaNextStudioBlazoeOptions.BackendApiConfig);

        context.Services.AddWorkflowsModule();
        context.Services.AddSettingsModule();

        Configure<DesignerOptions>(configuration.GetSection("DesignerOptions"));

        var abpLocalizationOptions = context.Services.ExecutePreConfiguredActions<AbpLocalizationOptions>();
        context.Services.AddLocalizationModule(new LocalizationConfig
        {
            ConfigureLocalizationOptions = options =>
            {
                options.SupportedCultures = abpLocalizationOptions.Languages.Select(x => x.CultureName).ToArray();
            }
        });
        context.Services.AddElsaStudioTranslations();
    }
}
