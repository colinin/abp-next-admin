using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.TaskManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.TaskManagement;

[DependsOn(typeof(AbpValidationModule))]
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
public class TaskManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TaskManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TaskManagementResource>()
                .AddVirtualJson("/LINGYUN/Abp/TaskManagement/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(TaskManagementErrorCodes.Namespace, typeof(TaskManagementResource));
        });
    }
}