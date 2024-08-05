using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement;

[RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
[Area("oss-management")]
[Route("api/oss-management/objects")]
public class OssObjectController : AbpControllerBase, IOssObjectAppService
{
    protected IFileUploader FileUploader { get; }
    protected IOssObjectAppService OssObjectAppService { get; }

    public OssObjectController(
        IFileUploader fileUploader,
        IOssObjectAppService ossObjectAppService)
    {
        FileUploader = fileUploader;
        OssObjectAppService = ossObjectAppService;
    }

    [HttpPost]
    public async virtual Task<OssObjectDto> CreateAsync([FromForm] CreateOssObjectInput input)
    {
        return await OssObjectAppService.CreateAsync(input);
    }

    [HttpPost]
    [Route("upload")]
    [DisableAuditing]
    [Authorize(AbpOssManagementPermissions.OssObject.Create)]
    public async virtual Task UploadAsync([FromForm] UploadFileChunkInput input)
    {
        await FileUploader.UploadAsync(input);
    }

    [HttpPost]
    [Route("bulk-delete")]
    public async virtual Task BulkDeleteAsync(BulkDeleteOssObjectInput input)
    {
        await OssObjectAppService.BulkDeleteAsync(input);
    }

    [HttpDelete]
    public async virtual Task DeleteAsync(GetOssObjectInput input)
    {
        await OssObjectAppService.DeleteAsync(input);
    }

    [HttpGet]
    public async virtual Task<OssObjectDto> GetAsync(GetOssObjectInput input)
    {
        return await OssObjectAppService.GetAsync(input);
    }

    [HttpGet]
    [Route("download")]
    public async virtual Task<IRemoteStreamContent> GetContentAsync(GetOssObjectInput input)
    {
        return await OssObjectAppService.GetContentAsync(input);
    }
}
