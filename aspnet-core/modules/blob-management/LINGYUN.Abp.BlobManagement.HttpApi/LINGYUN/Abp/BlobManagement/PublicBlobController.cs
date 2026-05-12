using LINGYUN.Abp.BlobManagement.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement;

[Controller]
[Area(BlobManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = BlobManagementRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{BlobManagementRemoteServiceConsts.ModuleName}/public/blobs")]
public class PublicBlobController : BlobControllerBase, IPublicBlobAppService
{

    private readonly IPublicBlobAppService _service;

    public PublicBlobController(IPublicBlobAppService service)
    {
        _service = service;
    }

    [HttpGet("uid/{id}/content")]
    public virtual Task<IRemoteStreamContent> GetContentAsync(Guid id)
    {
        return _service.GetContentAsync(id);
    }

    [HttpGet("content/{name}")]
    public virtual Task<IRemoteStreamContent> GetContentByNameAsync(string name)
    {
        return _service.GetContentByNameAsync(name);
    }

    [HttpGet("{id}")]
    public virtual Task<BlobDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<BlobDto>> GetListAsync(BlobGetPagedListWithoutContainerInput input)
    {
        return _service.GetListAsync(input);
    }
}
