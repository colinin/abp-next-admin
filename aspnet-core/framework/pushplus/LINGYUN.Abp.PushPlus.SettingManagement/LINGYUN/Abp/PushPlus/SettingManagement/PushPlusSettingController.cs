using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.PushPlus.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area(AbpSettingManagementRemoteServiceConsts.ModuleName)]
    [Route($"api/{AbpSettingManagementRemoteServiceConsts.ModuleName}/push-plus")]
    public class PushPlusSettingController : AbpControllerBase, IPushPlusSettingAppService
    {
        protected IPushPlusSettingAppService Service { get; }

        public PushPlusSettingController(
            IPushPlusSettingAppService service)
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
