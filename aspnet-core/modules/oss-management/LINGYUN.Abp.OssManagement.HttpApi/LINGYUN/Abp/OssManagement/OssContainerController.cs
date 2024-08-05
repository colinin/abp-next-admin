using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.OssManagement;

[RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
[Area("oss-management")]
[Route("api/oss-management/containes")]
public class OssContainerController : AbpControllerBase, IOssContainerAppService
{
    protected IOssContainerAppService OssContainerAppService { get; }

    public OssContainerController(
        IOssContainerAppService ossContainerAppService)
    {
        OssContainerAppService = ossContainerAppService;
    }

    [HttpPost]
    [Route("{name}")]
    public async virtual Task<OssContainerDto> CreateAsync(string name)
    {
        return await OssContainerAppService.CreateAsync(name);
    }

    [HttpDelete]
    [Route("{name}")]
    public async virtual Task DeleteAsync(string name)
    {
        await OssContainerAppService.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    public async virtual Task<OssContainerDto> GetAsync(string name)
    {
        return await OssContainerAppService.GetAsync(name);
    }

    [HttpGet]
    public async virtual Task<OssContainersResultDto> GetListAsync(GetOssContainersInput input)
    {
        return await OssContainerAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("objects")]
    public async virtual Task<OssObjectsResultDto> GetObjectListAsync(GetOssObjectsInput input)
    {
        return await OssContainerAppService.GetObjectListAsync(input);
    }
}
