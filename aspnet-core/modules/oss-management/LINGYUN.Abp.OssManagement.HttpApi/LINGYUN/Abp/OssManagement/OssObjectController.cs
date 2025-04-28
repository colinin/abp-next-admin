using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
    [Authorize(AbpOssManagementPermissions.OssObject.Create)]
    public virtual Task<OssObjectDto> CreateAsync([FromForm] CreateOssObjectInput input)
    {
        return OssObjectAppService.CreateAsync(input);
    }

    [HttpPost]
    [Route("upload")]
    [DisableAuditing]
    [Authorize(AbpOssManagementPermissions.OssObject.Create)]
    public virtual Task UploadAsync([FromForm] UploadFileChunkInput input)
    {
        return FileUploader.UploadAsync(input);
    }

    [HttpPost]
    [Route("bulk-delete")]
    [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
    public virtual Task BulkDeleteAsync(BulkDeleteOssObjectInput input)
    {
        return OssObjectAppService.BulkDeleteAsync(input);
    }

    [HttpDelete]
    [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
    public virtual Task DeleteAsync(GetOssObjectInput input)
    {
        return OssObjectAppService.DeleteAsync(input);
    }

    [HttpGet]
    [Authorize(AbpOssManagementPermissions.OssObject.Default)]
    public virtual Task<OssObjectDto> GetAsync(GetOssObjectInput input)
    {
        return OssObjectAppService.GetAsync(input);
    }

    [HttpGet]
    [Route("download")]
    [Authorize(AbpOssManagementPermissions.OssObject.Download)]
    [Obsolete("请使用 GenerateUrlAsync 与 DownloadAsync的组合")]
    public virtual Task<IRemoteStreamContent> GetContentAsync(GetOssObjectInput input)
    {
        return OssObjectAppService.GetContentAsync(input);
    }

    [HttpGet]
    [Route("generate-url")]
    [Authorize(AbpOssManagementPermissions.OssObject.Download)]
    public virtual Task<string> GenerateUrlAsync(GetOssObjectInput input)
    {
        return OssObjectAppService.GenerateUrlAsync(input);
    }

    [HttpGet]
    [Route("download/{urlKey}")]
    public virtual Task<IRemoteStreamContent> DownloadAsync(string urlKey)
    {
        return OssObjectAppService.DownloadAsync(urlKey);
    }
}
