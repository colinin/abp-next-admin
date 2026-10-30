using Elsa.Common.Multitenancy;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Workflows;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Multitenancy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext;

[DependsOn(
    typeof(AbpFeaturesModule),
    typeof(AbpThreadingModule),
    typeof(AbpJsonModule))]
public class AbpElsaNextModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<ITenantResolver, AbpTenantResolver>();

        var elsaModule = context.Services.GetPreConfigureActions<IModule>();

        context.Services.AddElsa(elsa =>
        {
            elsa
             .AddActivitiesFrom<AbpElsaNextModule>()
             .AddWorkflowsFrom<AbpElsaNextModule>();

            elsaModule.Configure(elsa);
        });

        context.Services.Replace(
            ServiceDescriptor.Singleton<IIdentityGenerator, AbpElsaIdentityGenerator>());

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ElsaNextResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/Localization/Resources");
        });
    }
}
