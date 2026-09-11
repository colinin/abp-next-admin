using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Email.Extensions;
using LINGYUN.Abp.ElsaNext.Localization;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Email;

[DependsOn(
    typeof(AbpEmailingModule),
    typeof(AbpElsaNextModule))]
public class AbpElsaNextEmailModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAbpEmail();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextEmailModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/Email/Localization/Resources");
        });
    }
}
