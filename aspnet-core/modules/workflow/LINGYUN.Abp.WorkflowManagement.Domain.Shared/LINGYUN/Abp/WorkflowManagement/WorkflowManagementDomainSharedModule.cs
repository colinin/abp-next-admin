using LINGYUN.Abp.WorkflowManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WorkflowManagement
{
    [DependsOn(
       typeof(AbpLocalizationModule))]
    public class WorkflowManagementDomainSharedModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<WorkflowManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<WorkflowManagementResource>()
                    .AddVirtualJson("/LINGYUN/Abp/WorkflowManagement/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(WorkflowManagementErrorCodes.Namespace, typeof(WorkflowManagementResource));
            });
        }
    }
}
