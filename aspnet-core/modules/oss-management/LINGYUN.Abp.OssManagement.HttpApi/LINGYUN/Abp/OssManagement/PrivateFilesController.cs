using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement;

[RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
[Area("oss-management")]
[Route("api/files/private")]
public class PrivateFilesController : AbpControllerBase, IPrivateFileAppService
{
    private readonly IPrivateFileAppService _service;

    public PrivateFilesController(
        IPrivateFileAppService service)
    {
        _service = service;

        LocalizationResource = typeof(AbpOssManagementResource);
    }

    [HttpPost]
    public async virtual Task<OssObjectDto> UploadAsync([FromForm] UploadFileInput input)
    {
        return await _service.UploadAsync(input);
    }

    [HttpPost]
    [Route("upload")]
    public async virtual Task UploadAsync([FromForm] UploadFileChunkInput input)
    {
        await _service.UploadAsync(input);
    }

    [HttpGet]
    [Route("search")]
    public async virtual Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
    {
        return await _service.GetListAsync(input);
    }

    [HttpGet]
    [Route("{Name}")]
    [Route("{Name}/{Process}")]
    [Route("p/{Path}/{Name}")]
    [Route("p/{Path}/{Name}/{Process}")]
    [Route("t/{TenantId}/{Name}")]
    [Route("t/{TenantId}/{Name}/{Process}")]
    [Route("t/{TenantId}/p/{Path}/{Name}")]
    [Route("t/{TenantId}/p/{Path}/{Name}/{Process}")]
    public async virtual Task<IRemoteStreamContent> GetAsync([FromRoute] GetPublicFileInput input)
    {
        using (CurrentTenant.Change(input.GetTenantId(CurrentTenant)))
        {
            return await _service.GetAsync(input);
        }
    }

    [HttpDelete]
    public async virtual Task DeleteAsync(GetPublicFileInput input)
    {
        await _service.DeleteAsync(input);
    }

    [HttpGet]
    [Route("share")]
    public async virtual Task<ListResultDto<MyFileShareDto>> GetShareListAsync()
    {
        return await _service.GetShareListAsync();
    }

    [HttpPost]
    [Route("share")]
    public async virtual Task<FileShareDto> ShareAsync(FileShareInput input)
    {
        return await _service.ShareAsync(input);
    }
}
