using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.SettingManagement
{
    [Authorize(AbpSettingManagementPermissions.Settings.Default)]
    public class SettingAppService : ApplicationService, ISettingAppService
    {
        protected ISettingManager SettingManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        public SettingAppService(ISettingManager settingManager,
            ISettingDefinitionManager settingDefinitionManager)
        {
            SettingManager = settingManager;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationResource = typeof(AbpSettingManagementResource);
        }

        public virtual async Task<ListResultDto<SettingDto>> GetAsync([NotNull] string providerName, [NotNull] string providerKey)
        {
            var settingsDto = new List<SettingDto>();
            var settingDefinitions = SettingDefinitionManager.GetAll();
            foreach(var setting in settingDefinitions)
            {
                if (setting.Providers.Any() && !setting.Providers.Contains(providerName))
                {
                    continue;
                }

                var settingValue = await SettingManager.GetOrNullAsync(setting.Name, providerName, providerKey);
                var settingInfo = new SettingDto
                {
                    Name = setting.Name,
                    Value = settingValue,
                    DefaultValue = setting.DefaultValue,
                    Description = setting.Description.Localize(StringLocalizerFactory),
                    DisplayName = setting.DisplayName.Localize(StringLocalizerFactory)
                };
                settingsDto.Add(settingInfo);
            }
            return new ListResultDto<SettingDto>(settingsDto);
        }

        [Authorize(AbpSettingManagementPermissions.Settings.Update)]
        public virtual async Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateSettingsDto input)
        {
            foreach (var setting in input.Settings)
            {
                await SettingManager.SetAsync(setting.Name, providerName, providerKey, setting.Value);
            }
        }
    }
}
