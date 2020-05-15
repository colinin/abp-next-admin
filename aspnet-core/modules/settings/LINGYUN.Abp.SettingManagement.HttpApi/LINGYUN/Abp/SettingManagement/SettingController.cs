using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.SettingManagement
{
    [RemoteService(Name = AbpSettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("abp")]
    public class SettingController : AbpController, ISettingAppService
    {
        private readonly ISettingAppService _settingAppService;
        public SettingController(ISettingAppService settingAppService)
        {
            _settingAppService = settingAppService;
        }

        public virtual async Task<ListResultDto<SettingDto>> GetAsync([NotNull] string providerName, [NotNull] string providerKey)
        {
            return await _settingAppService.GetAsync(providerName, providerKey);
        }

        public virtual async Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateSettingsDto input)
        {
            await _settingAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}
