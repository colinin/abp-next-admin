using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.Minio;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinioOssContainerServiceCollectionExtensions
{
    public static IServiceCollection AddMinioContainer(this IServiceCollection services)
    {
        services.AddTransient<IOssContainerFactory, MinioOssContainerFactory>();

        services.AddTransient<IOssObjectExpireor>(provider =>
            provider
                .GetRequiredService<IOssContainerFactory>()
                .Create()
                .As<MinioOssContainer>());

        return services;
    }
}
