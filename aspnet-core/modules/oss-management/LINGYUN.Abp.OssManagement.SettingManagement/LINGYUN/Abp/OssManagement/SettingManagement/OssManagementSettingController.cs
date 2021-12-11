using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.OssManagement.SettingManagement
{
    [RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("settingManagement")]
    [Route("api/setting-management/oss-management")]
    public class OssManagementSettingController : AbpController, IOssManagementSettingAppService
    {
        protected IOssManagementSettingAppService WeChatSettingAppService { get; }

        public OssManagementSettingController(
            IOssManagementSettingAppService weChatSettingAppService)
        {
            WeChatSettingAppService = weChatSettingAppService;
        }

        [HttpGet]
        [Route("by-current-tenant")]
        public virtual async Task<SettingGroupResult> GetAllForCurrentTenantAsync()
        {
            return await WeChatSettingAppService.GetAllForCurrentTenantAsync();
        }

        [HttpGet]
        [Route("by-global")]
        public virtual async Task<SettingGroupResult> GetAllForGlobalAsync()
        {
            return await WeChatSettingAppService.GetAllForGlobalAsync();
        }
    }
}
