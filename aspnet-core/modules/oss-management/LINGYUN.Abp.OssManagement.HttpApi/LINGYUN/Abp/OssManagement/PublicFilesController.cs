using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement;

[RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
[Area("oss-management")]
[Route("api/files/public")]
public class PublicFilesController : AbpControllerBase, IPublicFileAppService
{
    private readonly IPublicFileAppService _publicFileAppService;

    public PublicFilesController(
        IPublicFileAppService publicFileAppService)
    {
        _publicFileAppService = publicFileAppService;

        LocalizationResource = typeof(AbpOssManagementResource);
    }


    [HttpPost]
    public async virtual Task<OssObjectDto> UploadAsync([FromForm] UploadFileInput input)
    {
        return await _publicFileAppService.UploadAsync(input);
    }

    [HttpPost]
    [Route("upload")]
    public async virtual Task UploadAsync([FromForm] UploadFileChunkInput input)
    {
        await _publicFileAppService.UploadAsync(input);
    }

    [HttpGet]
    [Route("search")]
    public async virtual Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
    {
        return await _publicFileAppService.GetListAsync(input);
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
            return await _publicFileAppService.GetAsync(input);
        }
    }

    [HttpDelete]
    public async virtual Task DeleteAsync(GetPublicFileInput input)
    {
        await _publicFileAppService.DeleteAsync(input);
    }
}
