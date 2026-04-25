using LINGYUN.Abp.BlobManagement;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.BlobManagement;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpBlobManagementHttpApiClientModule))]
public class AbpBlobStoringBlobManagementModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("BlobManagement");

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                if (!string.Equals("blob-management", containerName))
                {
                    containerConfiguration.UseBlobManagement();
                }
            });
        });
    }
}
