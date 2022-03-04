using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Tencent.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area("settingManagement")]
[Route("api/setting-management/tencent-cloud")]
public class TenantCloudSettingController : AbpControllerBase, ITenantCloudSettingAppService
{
    protected ITenantCloudSettingAppService Service { get; }

    public TenantCloudSettingController(ITenantCloudSettingAppService service)
    {
        Service = service;
    }

    [HttpGet]
    [Route("by-current-tenant")]
    public Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return Service.GetAllForCurrentTenantAsync();
    }

    [HttpGet]
    [Route("by-global")]
    public Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return Service.GetAllForGlobalAsync();
    }
}
