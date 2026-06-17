using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Settings;
using ValueType = Volo.Abp.Settings.ValueType;

namespace LINGYUN.Abp.SettingManagement;

public abstract class SettingV2AppServiceBase : ApplicationService
{
    protected IDistributedEventBus EventBus { get; }
    protected ISettingManager SettingManager { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }

    protected SettingV2AppServiceBase(
        IDistributedEventBus eventBus,
        ISettingManager settingManager,
        ISettingDefinitionManager settingDefinitionManager)
    {
        EventBus = eventBus;
        SettingManager = settingManager;
        SettingDefinitionManager = settingDefinitionManager;

        LocalizationResource = typeof(AbpSettingManagementResource);
    }

    protected async virtual Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
    {
        var result = new SettingGroupResult();

        var settingDefines = await SettingDefinitionManager.GetAllAsync();


        SettingGroupDto CreateSettingGroup(SettingResourceDefinition resource)
        {
            return resource != null
                ? new SettingGroupDto(resource.Localize(StringLocalizerFactory).Value)
                : new SettingGroupDto(L["Settings:DefaultGroup"]);
        }
        SettingDto CreateSetting(SettingGroupDto groupDto, SettingResourceDefinition resource)
        {
            var settingDisplayName = resource != null
                ? resource.Localize(StringLocalizerFactory).Value
                : L["Settings:DefaultSetting"];
            return groupDto.AddSetting(settingDisplayName, settingDisplayName);
        }
        async Task<bool> IsEnabledSettingResource(SettingResourceDefinition resource)
        {
            if (resource?.RequiredFeatures != null)
            {
                var checkFeatures = await FeatureChecker.IsEnabledAsync(resource.RequiredFeatures);
                if (!checkFeatures.Any(x => x.Value))
                {
                    return false;
                }
            }
            if (resource?.RequiredPermissions != null &&
                !await AuthorizationService.IsGrantedAnyAsync(resource.RequiredPermissions))
            {
                return false;
            }
            return true;
        }
        async Task<bool> IsEnabledSetting(SettingDefinition setting)
        {
            if (setting.Providers.Count > 0 && !setting.Providers.Any(provider => provider == providerName))
            {
                return false;
            }
            var requiredFeatures = setting.GetRequiredFeatures();
            if (requiredFeatures.Any())
            {
                var checkFeatures = await FeatureChecker.IsEnabledAsync(requiredFeatures.ToArray());
                if (!checkFeatures.Any(x => x.Value))
                {
                    return false;
                }
            }
            var requiredPermissions = setting.GetRequiredPermissions();
            if (requiredPermissions.Any() &&
                !await AuthorizationService.IsGrantedAnyAsync(requiredPermissions.ToArray()))
            {
                return false;
            }
            return true;
        }

        foreach (var settingGroups in settingDefines.GroupBy(x => x.GetGroupOrNull()).OrderBy(x => x.Key?.Order ?? 9999))
        {
            if (!await IsEnabledSettingResource(settingGroups.Key))
            {
                continue;
            }
            var groupDto = CreateSettingGroup(settingGroups.Key);
            foreach (var settings in settingGroups.GroupBy(x => x.GetParentOrNull()).OrderBy(x => x.Key?.Order ?? 9999))
            {
                if (!await IsEnabledSettingResource(settings.Key))
                {
                    continue;
                }
                var settingDto = CreateSetting(groupDto, settings.Key);

                foreach (var setting in settings)
                {
                    if (!await IsEnabledSetting(setting))
                    {
                        continue;
                    }
                    var valueType = setting.GetValueTypeOrDefault();

                    var settingDetailsDto = settingDto.AddDetail(
                        await SettingDefinitionManager.GetAsync(setting.Name),
                        StringLocalizerFactory,
                        await SettingManager.GetOrNullAsync(setting.Name, providerName, providerKey),
                        valueType,
                        providerName);

                    if (valueType == ValueType.Option)
                    {
                        var options = setting.GetOptions();
                        settingDetailsDto.AddOptions(options.Select(option => new OptionDto(option.Name, option.Value)));
                    }

                    var slot = setting.GetSlotOrNull();
                    if (!slot.IsNullOrWhiteSpace())
                    {
                        settingDetailsDto.WithSlot(slot);
                    }

                    var requiredFeatures = setting.GetRequiredFeatures();
                    if (requiredFeatures.Any())
                    {
                        settingDetailsDto.RequiredFeature(requiredFeatures.ToArray());
                    }

                    var requiredPermissions = setting.GetRequiredPermissions();
                    if (requiredPermissions.Any())
                    {
                        settingDetailsDto.RequiredPermission(requiredPermissions.ToArray());
                    }
                }

                if (settingDto.Details.Count == 0)
                {
                    groupDto.Settings.Remove(settingDto);
                }
            }

            result.AddGroup(groupDto);
        }

        return result;
    }

    protected async virtual Task CheckFeatureAsync()
    {
        await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
    }
}
