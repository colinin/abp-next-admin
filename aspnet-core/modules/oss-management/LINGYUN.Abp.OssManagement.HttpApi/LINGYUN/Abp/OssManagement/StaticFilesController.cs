using LINGYUN.Abp.OssManagement.Localization;
using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement;

[RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
[Area("oss-management")]
[Route("api/files/static")]
public class StaticFilesController : AbpControllerBase, IStaticFilesAppService
{
    private readonly IOssObjectAppService _ossObjectAppService;
    private readonly IStaticFilesAppService _staticFilesAppServic;

    public StaticFilesController(
        IOssObjectAppService ossObjectAppService,
        IStaticFilesAppService staticFilesAppServic)
    {
        _ossObjectAppService = ossObjectAppService;
        _staticFilesAppServic = staticFilesAppServic;

        LocalizationResource = typeof(AbpOssManagementResource);
    }

    [HttpPost]
    [Authorize(AbpOssManagementPermissions.OssObject.Create)]
    public async virtual Task<OssObjectDto> UploadAsync([FromForm] CreateOssObjectInput input)
    {
        return await _ossObjectAppService.CreateAsync(input);
    }

    [HttpGet]
    [Route("{Bucket}/{Name}")]
    [Route("{Bucket}/{Name}/{Process}")]
    [Route("{Bucket}/p/{Path}/{Name}")]
    [Route("{Bucket}/p/{Path}/{Name}/{Process}")]
    [Route("t/{TenantId}/{Bucket}/{Name}")]
    [Route("t/{TenantId}/{Bucket}/{Name}/{Process}")]
    [Route("t/{TenantId}/{Bucket}/p/{Path}/{Name}")]
    [Route("t/{TenantId}/{Bucket}/p/{Path}/{Name}/{Process}")]
    public async virtual Task<IRemoteStreamContent> GetAsync([FromRoute] GetStaticFileInput input)
    {
        using (CurrentTenant.Change(input.GetTenantId(CurrentTenant)))
        {
            return await _staticFilesAppServic.GetAsync(input);
        }
    }
}
