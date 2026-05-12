using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Integration.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement.Integration;

[Controller]
[Area(BlobManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = BlobManagementRemoteServiceConsts.RemoteServiceName)]
[Route($"integration-api/{BlobManagementRemoteServiceConsts.ModuleName}/blobs")]
public class BlobIntegrationController : AbpControllerBase, IBlobIntegrationService
{
    private readonly IBlobIntegrationService _service;

    public BlobIntegrationController(IBlobIntegrationService service)
    {
        _service = service;
    }

    [HttpPost]
    public virtual Task<BlobDto> CreateAsync([FromForm] BlobFileCreateIntegrationDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    public virtual Task DeleteAsync(BlobFileGetByNameIntegrationDto input)
    {
        return _service.DeleteAsync(input);
    }

    [HttpGet("exists")]
    public virtual Task<bool> ExistsAsync(BlobFileGetByNameIntegrationDto input)
    {
        return _service.ExistsAsync(input);
    }

    [HttpGet("download")]
    public virtual Task<IRemoteStreamContent> GetContentAsync(BlobFileGetByNameIntegrationDto input)
    {
        return _service.GetContentAsync(input);
    }
}
