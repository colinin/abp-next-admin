using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.OssManagement;
public abstract class OssContainerBase : IOssContainer
{
    protected AbpOssManagementOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected OssContainerBase(
        IOptions<AbpOssManagementOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        Options = options.Value;
        ServiceScopeFactory = serviceScopeFactory;
    }

    public async virtual Task<OssObject> GetObjectAsync(GetOssObjectRequest request)
    {
        var ossObject = await GetOssObjectAsync(request);

        if (ShouldProcessObject(ossObject) && !request.Process.IsNullOrWhiteSpace())
        {
            using var serviceScope = ServiceScopeFactory.CreateScope();
            var context = new OssObjectProcesserContext(request.Process, ossObject, serviceScope.ServiceProvider);
            foreach (var processer in Options.Processers)
            {
                await processer.ProcessAsync(context);

                if (context.Handled)
                {
                    ossObject.SetContent(context.Content);
                    break;
                }
            }
        }

        return ossObject;
    }

    public async virtual Task DeleteAsync(string name)
    {
        if (Options.CheckStaticBucket(name))
        {
            throw new BusinessException(code: OssManagementErrorCodes.ContainerDeleteWithStatic);
        }

        await DeleteBucketAsync(name);
    }

    public abstract Task BulkDeleteObjectsAsync(BulkDeleteObjectRequest request);
    public abstract Task<OssContainer> CreateAsync(string name);
    public abstract Task<OssObject> CreateObjectAsync(CreateOssObjectRequest request);
    public abstract Task DeleteObjectAsync(GetOssObjectRequest request);
    public abstract Task<bool> ExistsAsync(string name);
    public abstract Task<OssContainer> GetAsync(string name);
    public abstract Task<GetOssContainersResponse> GetListAsync(GetOssContainersRequest request);
    public abstract Task<GetOssObjectsResponse> GetObjectsAsync(GetOssObjectsRequest request);
    protected abstract Task DeleteBucketAsync(string name);
    protected abstract Task<OssObject> GetOssObjectAsync(GetOssObjectRequest request);
    protected virtual bool ShouldProcessObject(OssObject ossObject)
    {
        return true;
    }
}
