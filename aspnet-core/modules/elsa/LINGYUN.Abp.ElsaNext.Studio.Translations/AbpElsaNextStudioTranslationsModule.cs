using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Translations;

/// <summary>
/// ABP-hosted localization for the Elsa Studio UI copy. Registers the texts extracted from the
/// official <c>Elsa.Studio.Translations</c> resx (en/zh-Hans only) as ABP virtual-JSON resources,
/// so any ABP server-side consumer can translate Elsa Studio UI copy through the standard .NET
/// <c>IStringLocalizer&lt;ElsaStudioTranslationsResource&gt;</c> API. Also provides
/// <see cref="ElsaStudioLocalizationProvider"/>, an Elsa <c>ILocalizationProvider</c> bridge that
/// performs translation via that .NET-standard localizer.
/// </summary>
[DependsOn(typeof(AbpLocalizationModule))]
public class AbpElsaNextStudioTranslationsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioTranslationsModule>("LINGYUN.Abp.ElsaNext.Studio.Translations");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ElsaStudioResource>("en")
                .AddVirtualJson("/Localization/Resources/ElsaStudio");
        });

        // When this module runs inside a host that renders Elsa Studio on the server (Blazor Server
        // integration), Elsa's ILocalizer resolves copy through our ABP-backed provider.
        context.Services.AddElsaStudioTranslationsIfNotRegistered();
    }
}
