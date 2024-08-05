using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement;

[Authorize(AbpOssManagementPermissions.Container.Default)]
public class OssContainerAppService : OssManagementApplicationServiceBase, IOssContainerAppService
{
    protected IOssContainerFactory OssContainerFactory { get; }

    public OssContainerAppService(
        IOssContainerFactory ossContainerFactory)
    {
        OssContainerFactory = ossContainerFactory;
    }

    [Authorize(AbpOssManagementPermissions.Container.Create)]
    public async virtual Task<OssContainerDto> CreateAsync(string name)
    {
        var oss = CreateOssContainer();
        var container = await oss.CreateAsync(name);

        return ObjectMapper.Map<OssContainer, OssContainerDto>(container);
    }

    [Authorize(AbpOssManagementPermissions.Container.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var oss = CreateOssContainer();

        await oss.DeleteAsync(name);
    }

    public async virtual Task<OssContainerDto> GetAsync(string name)
    {
        var oss = CreateOssContainer();
        var container = await oss.GetAsync(name);

        return ObjectMapper.Map<OssContainer, OssContainerDto>(container);
    }

    public async virtual Task<OssContainersResultDto> GetListAsync(GetOssContainersInput input)
    {
        var oss = CreateOssContainer();

        var containerResponse = await oss.GetListAsync(
            input.Prefix, input.Marker, input.SkipCount, input.MaxResultCount);

        return ObjectMapper.Map<GetOssContainersResponse, OssContainersResultDto>(containerResponse);
    }

    public async virtual Task<OssObjectsResultDto> GetObjectListAsync(GetOssObjectsInput input)
    {
        var oss = CreateOssContainer();

        var ossObjectResponse = await oss.GetObjectsAsync(
            input.Bucket, input.Prefix, input.Marker,
            input.Delimiter, input.EncodingType, input.MD5,
            input.SkipCount, input.MaxResultCount);

        return ObjectMapper.Map<GetOssObjectsResponse, OssObjectsResultDto>(ossObjectResponse);
    }

    protected virtual IOssContainer CreateOssContainer()
    {
        return OssContainerFactory.Create();
    }
}
