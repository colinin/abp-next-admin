using LINGYUN.Abp.BlobStoring.Tencent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OssManagement.Tencent;

public class TencentOssContainerFactory : IOssContainerFactory
{
    protected IClock Clock { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ICosClientFactory CosClientFactory { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<AbpOssManagementOptions> Options { get; }

    public TencentOssContainerFactory(
        IClock clock,
        ICurrentTenant currentTenant,
        ICosClientFactory cosClientFactory,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpOssManagementOptions> options)
    {
        Clock = clock;
        CurrentTenant = currentTenant;
        CosClientFactory = cosClientFactory;
        Options = options;
        ServiceScopeFactory = serviceScopeFactory;
    }

    public IOssContainer Create()
    {
        return new TencentOssContainer(
            Clock,
            CurrentTenant,
            CosClientFactory,
            ServiceScopeFactory,
            Options);
    }
}
