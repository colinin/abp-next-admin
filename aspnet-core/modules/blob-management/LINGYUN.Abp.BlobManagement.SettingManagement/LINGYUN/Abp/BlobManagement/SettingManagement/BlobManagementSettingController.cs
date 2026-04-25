using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.BlobManagement.SettingManagement;

[RemoteService(Name = BlobManagementRemoteServiceConsts.RemoteServiceName)]
[Area("settingManagement")]
[Route("api/setting-management/blob-management")]
public class BlobManagementSettingController : AbpControllerBase, IBlobManagementSettingAppService
{
    private readonly IBlobManagementSettingAppService _service;
    public BlobManagementSettingController(
        IBlobManagementSettingAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("by-current-tenant")]
    public virtual Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return _service.GetAllForCurrentTenantAsync();
    }

    [HttpGet]
    [Route("by-global")]
    public virtual Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return _service.GetAllForGlobalAsync();
    }
}
