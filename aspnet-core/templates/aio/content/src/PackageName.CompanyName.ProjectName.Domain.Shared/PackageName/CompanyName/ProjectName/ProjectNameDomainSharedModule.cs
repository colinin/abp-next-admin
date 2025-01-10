using PackageName.CompanyName.ProjectName.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpLocalizationModule))]
public class ProjectNameDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProjectNameDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ProjectNameResource>()
                .AddVirtualJson("/PackageName/CompanyName/ProjectName/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(ProjectNameErrorCodes.Namespace, typeof(ProjectNameResource));
        });
    }
}
