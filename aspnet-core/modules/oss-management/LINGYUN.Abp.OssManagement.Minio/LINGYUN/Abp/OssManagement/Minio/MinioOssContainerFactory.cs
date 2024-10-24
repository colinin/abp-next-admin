using Microsoft.Extensions.Logging;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OssManagement.Minio;
public class MinioOssContainerFactory : IOssContainerFactory
{
    protected IMinioBlobNameCalculator MinioBlobNameCalculator { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    protected IClock Clock { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ILogger<MinioOssContainer> Logger { get; }

    public MinioOssContainerFactory(
       IClock clock,
        ICurrentTenant currentTenant,
       ILogger<MinioOssContainer> logger,
       IMinioBlobNameCalculator minioBlobNameCalculator,
       IBlobNormalizeNamingService blobNormalizeNamingService,
       IBlobContainerConfigurationProvider configurationProvider)
    {
        Clock = clock;
        Logger = logger;
        CurrentTenant = currentTenant;
        MinioBlobNameCalculator = minioBlobNameCalculator;
        BlobNormalizeNamingService = blobNormalizeNamingService;
        ConfigurationProvider = configurationProvider;
    }

    public IOssContainer Create()
    {
        return new MinioOssContainer(
            Clock,
            CurrentTenant,
            Logger,
            MinioBlobNameCalculator,
            BlobNormalizeNamingService,
            ConfigurationProvider);
    }
}
