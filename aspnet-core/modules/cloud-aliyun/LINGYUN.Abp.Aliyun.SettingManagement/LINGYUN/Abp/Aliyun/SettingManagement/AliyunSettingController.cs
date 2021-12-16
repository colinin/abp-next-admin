using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Aliyun.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("settingManagement")]
    [Route("api/setting-management/aliyun")]
    public class AliyunSettingController : AbpController, IAliyunSettingAppService
    {
        protected IAliyunSettingAppService AppService { get; }

        public AliyunSettingController(
            IAliyunSettingAppService appService)
        {
            AppService = appService;
        }

        [HttpGet]
        [Route("by-current-tenant")]
        public virtual async Task<SettingGroupResult> GetAllForCurrentTenantAsync()
        {
            return await AppService.GetAllForCurrentTenantAsync();
        }

        [HttpGet]
        [Route("by-global")]
        public virtual async Task<SettingGroupResult> GetAllForGlobalAsync()
        {
            return await AppService.GetAllForGlobalAsync();
        }
    }
}
