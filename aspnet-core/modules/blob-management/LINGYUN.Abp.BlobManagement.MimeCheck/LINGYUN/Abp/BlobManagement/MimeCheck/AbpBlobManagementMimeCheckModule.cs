using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.MimeCheck;

[DependsOn(typeof(AbpBlobManagementDomainModule))]
public class AbpBlobManagementMimeCheckModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBlobManagementContentTypeResolveOptions>(options =>
        {
            options.BlobContentTypeResolvers.Add(new MimeCheckBlobContentTypeResolveContributor());
        });
    }
}
