using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("settingManagement")]
    [Route("api/setting-management/wechat")]
    public class WeChatSettingController : AbpController, IWeChatSettingAppService
    {
        protected IWeChatSettingAppService WeChatSettingAppService { get; }

        public WeChatSettingController(
            IWeChatSettingAppService weChatSettingAppService)
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
