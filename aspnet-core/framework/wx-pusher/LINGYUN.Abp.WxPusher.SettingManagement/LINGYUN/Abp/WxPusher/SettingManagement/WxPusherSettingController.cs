using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WxPusher.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area(AbpSettingManagementRemoteServiceConsts.ModuleName)]
    [Route($"api/{AbpSettingManagementRemoteServiceConsts.ModuleName}/wx-pusher")]
    public class WxPusherSettingController : AbpControllerBase, IWxPusherSettingAppService
    {
        protected IWxPusherSettingAppService Service { get; }

        public WxPusherSettingController(
            IWxPusherSettingAppService service)
        {
            Service = service;
        }

        [HttpGet]
        [Route("by-current-tenant")]
        public async virtual Task<SettingGroupResult> GetAllForCurrentTenantAsync()
        {
            return await Service.GetAllForCurrentTenantAsync();
        }

        [HttpGet]
        [Route("by-global")]
        public async virtual Task<SettingGroupResult> GetAllForGlobalAsync()
        {
            return await Service.GetAllForGlobalAsync();
        }
    }
}
