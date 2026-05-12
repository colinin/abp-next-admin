using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.FileSystem;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpBlobStoringFileSystemModule))]
public class AbpBlobManagementFileSystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("BlobManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            string.Equals(FileSystemBlobProvider.ProviderName, ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<BlobManagementContainer>((containerConfiguration) =>
                {
                    containerConfiguration.UseFileSystem(oss =>
                    {
                        var blobConfiguration = ossConfiguration.GetSection(FileSystemBlobProvider.ProviderName);

                        oss.BasePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            blobConfiguration["BasePath"] ?? "blobs");
                        oss.AppendContainerNameToBasePath = blobConfiguration.GetValue("AppendContainerNameToBasePath", true);
                    });
                });
            });
            context.Services.AddTransient<IBlobProvider, FileSystemBlobProvider>();
        }
    }
}
