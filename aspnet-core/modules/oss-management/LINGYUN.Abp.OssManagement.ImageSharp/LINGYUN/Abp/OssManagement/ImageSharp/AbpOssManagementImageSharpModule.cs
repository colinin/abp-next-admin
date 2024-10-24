using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.ImageSharp;

[DependsOn(typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementImageSharpModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOssManagementOptions>(options =>
        {
            options.AddProcesser(new ImageSharpOssObjectProcesser());
        });
    }
}
