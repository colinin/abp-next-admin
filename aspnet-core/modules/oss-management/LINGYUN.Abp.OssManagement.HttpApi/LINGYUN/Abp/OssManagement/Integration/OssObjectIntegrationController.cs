using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement.Integration;

[Area(OssManagementRemoteServiceConsts.ModuleName)]
[ControllerName("OssObjectIntegration")]
[RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
[Route($"integration-api/{OssManagementRemoteServiceConsts.ModuleName}/objects")]
public class OssObjectIntegrationController : AbpControllerBase, IOssObjectIntegrationService
{
    private readonly IOssObjectIntegrationService _service;
    public OssObjectIntegrationController(IOssObjectIntegrationService service)
    {
        _service = service;
    }

    [HttpPost]
    public virtual Task<OssObjectDto> CreateAsync([FromForm] CreateOssObjectInput input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    public virtual Task DeleteAsync(GetOssObjectInput input)
    {
        return _service.DeleteAsync(input);
    }

    [HttpGet]
    [Route("exists")]
    public virtual Task<GetOssObjectExistsResult> ExistsAsync(GetOssObjectInput input)
    {
        return _service.ExistsAsync(input);
    }

    [HttpGet]
    [Route("download")]
    public virtual Task<IRemoteStreamContent> GetAsync(GetOssObjectInput input)
    {
        return _service.GetAsync(input);
    }
}
