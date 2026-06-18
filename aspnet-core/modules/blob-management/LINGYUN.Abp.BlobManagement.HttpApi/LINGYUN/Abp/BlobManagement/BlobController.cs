using LINGYUN.Abp.BlobManagement.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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

    [HttpGet("download/{id}")]
    [HttpGet("download/t/{tenantId}/{id}")]
    public virtual Task<IRemoteStreamContent> DownloadAsync(BlobDownloadByIdInput input)
    {
        return _service.DownloadAsync(input);
    }

    [HttpGet("preview/{id}")]
    [HttpGet("preview/t/{tenantId}/{id}")]
    public async virtual Task<IRemoteStreamContent> PreviewAsync(BlobDownloadByIdInput input)
    {
        var content = await _service.PreviewAsync(input);

        if (!content.FileName.IsNullOrWhiteSpace())
        {
            var contentDisposition = new ContentDispositionHeaderValue("inline");
            contentDisposition.SetHttpFileName(content.FileName);
            HttpContext.Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
        }

        return content;
    }

    [HttpGet("download/by-name/{name}")]
    [HttpGet("download/by-name/t/{tenantId}/{name}")]
    public virtual Task<IRemoteStreamContent> DownloadByNameAsync(BlobDownloadByNameInput input)
    {
        return _service.DownloadByNameAsync(input);
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

    [HttpGet("generate/download-url/{id}")]
    public virtual Task<string> GenerateDownloadUrlAsync(Guid id)
    {
        return _service.GenerateDownloadUrlAsync(id);
    }

    [HttpGet("generate/preview-url/{id}")]
    public virtual Task<string> GeneratePreviewUrlAsync(Guid id)
    {
        return _service.GeneratePreviewUrlAsync(id);
    }
}
