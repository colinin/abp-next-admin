using LINGYUN.Abp.ElsaNext.ApprovalFlow.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

[DependsOn(typeof(AbpValidationModule))]
public class AbpElsaNextApprovalFlowDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextApprovalFlowDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ElsaNextApprovalFlowResource>()
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/ApprovalFlow/Localization/Domain");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(ElsaNextApprovalFlowErrorCodes.Namespace, typeof(ElsaNextApprovalFlowResource));
        });
    }
}
