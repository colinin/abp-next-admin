using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.FileSystem;

[DependsOn(
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementFileSystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("OssManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            "FileSystem".Equals(ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseFileSystem(oss =>
                    {
                        oss.BasePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            ossConfiguration["Bucket"] ?? "blobs");
                        oss.AppendContainerNameToBasePath = ossConfiguration.GetValue("AppendContainerNameToBasePath", true);
                    });
                });
            });
            context.Services.AddFileSystemContainer();
        }
    }
}
