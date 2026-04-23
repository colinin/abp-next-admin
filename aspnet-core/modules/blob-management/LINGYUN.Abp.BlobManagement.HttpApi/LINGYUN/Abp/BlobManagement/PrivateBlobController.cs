using LINGYUN.Abp.BlobManagement.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement;

[Authorize]
[Controller]
[Area(BlobManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = BlobManagementRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{BlobManagementRemoteServiceConsts.ModuleName}/private/blobs")]
public class PrivateBlobController : BlobControllerBase, IPrivateBlobAppService
{

    private readonly IPrivateBlobAppService _service;

    public PrivateBlobController(IPrivateBlobAppService service)
    {
        _service = service;
    }

    [HttpPost("file")]
    public virtual Task<BlobDto> CreateFileAsync([FromForm] BlobFileCreateWithoutContainerDto input)
    {
        input.CompareMd5 ??= GetCompareMd5InHeader();
        return _service.CreateFileAsync(input);
    }

    [HttpPost("file/chunk")]
    public virtual Task CreateChunkFileAsync([FromForm] BlobFileChunkCreateWithoutContainerDto input)
    {
        input.CompareMd5 ??= GetCompareMd5InHeader();
        return _service.CreateChunkFileAsync(input);
    }

    [HttpPost("folder")]
    public virtual Task<BlobDto> CreateFolderAsync(BlobFolderCreateWithoutContainerDto input)
    {
        return _service.CreateFolderAsync(input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
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
