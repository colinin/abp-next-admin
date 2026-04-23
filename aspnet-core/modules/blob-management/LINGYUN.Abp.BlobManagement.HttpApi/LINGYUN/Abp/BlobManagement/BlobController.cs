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
[Route($"api/{BlobManagementRemoteServiceConsts.ModuleName}/blobs")]
public class BlobController : BlobControllerBase, IBlobAppService
{

    private readonly IBlobAppService _service;

    public BlobController(IBlobAppService service)
    {
        _service = service;
    }

    [HttpPost("file")]
    [IgnoreAntiforgeryToken]
    public virtual Task<BlobDto> CreateFileAsync([FromForm] BlobFileCreateDto input)
    {
        input.CompareMd5 ??= GetCompareMd5InHeader();
        return _service.CreateFileAsync(input);
    }

    [HttpPost("file/chunk")]
    [IgnoreAntiforgeryToken]
    public virtual Task CreateChunkFileAsync([FromForm] BlobFileChunkCreateDto input)
    {
        input.CompareMd5 ??= GetCompareMd5InHeader();
        return _service.CreateChunkFileAsync(input);
    }

    [HttpPost("folder")]
    public virtual Task<BlobDto> CreateFolderAsync(BlobFolderCreateDto input)
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

    [HttpGet("content")]
    public virtual Task<IRemoteStreamContent> GetContentByNameAsync(BlobDownloadByNameInput input)
    {
        return _service.GetContentByNameAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<BlobDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<BlobDto>> GetListAsync(BlobGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }
}
