using LINGYUN.Abp.BlobStoring.Tencent;

namespace Microsoft.Extensions.DependencyInjection;
internal static class BlobStoringTencentHttpClientFactoryServiceCollectionExtensions
{
    public static IServiceCollection AddTenantOssClient(this IServiceCollection services)
    {
        services.AddHttpClient(BlobStoringTencentConsts.HttpClient);

        return services;
    }
}
