using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.TuiJuhe.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area(AbpSettingManagementRemoteServiceConsts.ModuleName)]
    [Route($"api/{AbpSettingManagementRemoteServiceConsts.ModuleName}/tui-juhe")]
    public class TuiJuheSettingController : AbpController, ITuiJuheSettingAppService
    {
        protected ITuiJuheSettingAppService Service { get; }

        public TuiJuheSettingController(
            ITuiJuheSettingAppService service)
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
