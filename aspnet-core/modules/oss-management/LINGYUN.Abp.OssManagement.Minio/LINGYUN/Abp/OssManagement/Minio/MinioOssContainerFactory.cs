using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<AbpOssManagementOptions> Options { get; }

    public MinioOssContainerFactory(
        IClock clock,
        ICurrentTenant currentTenant,
        ILogger<MinioOssContainer> logger,
        IMinioBlobNameCalculator minioBlobNameCalculator,
        IBlobNormalizeNamingService blobNormalizeNamingService,
        IBlobContainerConfigurationProvider configurationProvider,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpOssManagementOptions> options)
    {
        Clock = clock;
        Logger = logger;
        CurrentTenant = currentTenant;
        Options = options;
        ServiceScopeFactory = serviceScopeFactory;
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
            ConfigurationProvider,
            ServiceScopeFactory,
            Options);
    }
}
