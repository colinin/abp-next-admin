using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Localization;
using LINGYUN.Abp.BlobManagement.Permissions;
using LINGYUN.Abp.BlobManagement.Settings;
using LINGYUN.Abp.SettingManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using ValueType = LINGYUN.Abp.SettingManagement.ValueType;

namespace LINGYUN.Abp.BlobManagement.SettingManagement;

public class BlobManagementSettingAppService : ApplicationService, IBlobManagementSettingAppService
{
    protected ISettingManager SettingManager { get; }
    protected IPermissionChecker PermissionChecker { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }

    public BlobManagementSettingAppService(
        ISettingManager settingManager,
        IPermissionChecker permissionChecker,
        ISettingDefinitionManager settingDefinitionManager)
    {
        SettingManager = settingManager;
        PermissionChecker = permissionChecker;
        SettingDefinitionManager = settingDefinitionManager;
        LocalizationResource = typeof(BlobManagementResource);
    }

    public async virtual Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
    }

    public async virtual Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
    }

    protected async virtual Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string? providerKey = null)
    {
        var settingGroups = new SettingGroupResult();

        if (await FeatureChecker.IsEnabledAsync(BlobManagementFeatureNames.Blob.Enable) &&
            await PermissionChecker.IsGrantedAsync(BlobManagementPermissionNames.Blob.Default))
        {
            var ossSettingGroup = new SettingGroupDto(L["DisplayName:BlobManagement"], L["DisplayName:BlobManagement"]);

            var ossObjectSetting = ossSettingGroup.AddSetting(L["DisplayName:Blobs"], L["DisplayName:Blobs"]);

            ossObjectSetting.AddDetail(
                await SettingDefinitionManager.GetAsync(BlobManagementSettingNames.FileLimitLength),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(BlobManagementSettingNames.FileLimitLength, providerName, providerKey),
                ValueType.Number);
            ossObjectSetting.AddDetail(
                await SettingDefinitionManager.GetAsync(BlobManagementSettingNames.AllowFileExtensions),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(BlobManagementSettingNames.AllowFileExtensions, providerName, providerKey),
                ValueType.String);

            settingGroups.AddGroup(ossSettingGroup);
        }

        return settingGroups;
    }
}
