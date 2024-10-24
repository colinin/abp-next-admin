using LINGYUN.Abp.BlobStoring.Aliyun;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement.Aliyun;

public class AliyunOssContainerFactory : IOssContainerFactory
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IOssClientFactory OssClientFactory { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<AbpOssManagementOptions> Options { get; }

    public AliyunOssContainerFactory(
        ICurrentTenant currentTenant,
        IOssClientFactory ossClientFactory,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpOssManagementOptions> options)
    {
        Options = options;
        ServiceScopeFactory = serviceScopeFactory;
        CurrentTenant = currentTenant;
        OssClientFactory = ossClientFactory;
    }

    public IOssContainer Create()
    {
        return new AliyunOssContainer(
            CurrentTenant,
            OssClientFactory,
            ServiceScopeFactory,
            Options);
    }
}
