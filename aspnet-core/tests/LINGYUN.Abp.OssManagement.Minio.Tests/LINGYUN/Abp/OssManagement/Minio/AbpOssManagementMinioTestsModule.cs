using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Minio;

[DependsOn(
    typeof(AbpOssManagementMinioModule),
    typeof(AbpOssManagementDomainTestsModule))]
public class AbpOssManagementMinioTestsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configurationOptions = new AbpConfigurationBuilderOptions
        {
            BasePath = @"D:\Projects\Development\Abp\BlobStoring\Minio",
            EnvironmentName = "Test"
        };

        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseMinio(minio =>
                {
                    minio.BucketName = configuration[MinioBlobProviderConfigurationNames.BucketName];
                    minio.AccessKey = configuration[MinioBlobProviderConfigurationNames.AccessKey];
                    minio.SecretKey = configuration[MinioBlobProviderConfigurationNames.SecretKey];
                    minio.EndPoint = configuration[MinioBlobProviderConfigurationNames.EndPoint];
                });
            });
        });
    }
}
