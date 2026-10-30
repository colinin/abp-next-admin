using Elsa.ApprovalFlow.Features;
using Elsa.ApprovalFlow.Options;
using Elsa.Extensions;
using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Localization;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Activities;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextApprovalFlowActivitiesModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa");

        PreConfigure<IModule>(elsa =>
        {
            elsa.UseApprovalFlow(approvalFlow =>
            {
                approvalFlow.ConfigureOptions = options =>
                {
                    configuration.GetSection("ApprovalFlow").Bind(options);
                };
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextApprovalFlowActivitiesModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/ApprovalFlow/Activities/Localization/Resources");
        });
    }
}
