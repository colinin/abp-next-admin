using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Tencent.SettingManagement;

[RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area("settingManagement")]
[Route("api/setting-management/tencent-cloud")]
public class TencentCloudSettingController : AbpControllerBase, ITencentCloudSettingAppService
{
    protected ITencentCloudSettingAppService Service { get; }

    public TencentCloudSettingController(ITencentCloudSettingAppService service)
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
