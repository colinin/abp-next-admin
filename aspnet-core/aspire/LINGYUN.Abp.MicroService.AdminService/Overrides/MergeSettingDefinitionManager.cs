using System.Collections.Immutable;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.MicroService.AdminService.Overrides;

[Dependency(ReplaceServices = true)]
public class MergeSettingDefinitionManager : SettingDefinitionManager
{
    private readonly IStaticSettingDefinitionStore _staticStore;
    private readonly IDynamicSettingDefinitionStore _dynamicStore;

    public MergeSettingDefinitionManager(
        IStaticSettingDefinitionStore staticStore,
        IDynamicSettingDefinitionStore dynamicStore)
        : base(staticStore, dynamicStore)
    {
        _staticStore = staticStore;
        _dynamicStore = dynamicStore;
    }

    public async override Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        var staticSettings = await _staticStore.GetAllAsync();
        var dynamicSettings = await _dynamicStore.GetAllAsync();

        var mergedSettings = new Dictionary<string, SettingDefinition>();

        foreach (var staticSetting in staticSettings)
        {
            mergedSettings[staticSetting.Name] = staticSetting;
        }

        foreach (var dynamicSetting in dynamicSettings)
        {
            if (mergedSettings.TryGetValue(dynamicSetting.Name, out var existingSetting))
            {
                MergeSetting(existingSetting, dynamicSetting);
            }
            else
            {
                mergedSettings[dynamicSetting.Name] = dynamicSetting;
            }
        }

        return mergedSettings.Values.ToImmutableList();
    }

    private static void MergeSetting(SettingDefinition target, SettingDefinition source)
    {
        target.DisplayName = source.DisplayName;
        target.Description = source.Description ?? target.Description;
        target.DefaultValue = source.DefaultValue ?? target.DefaultValue;
        target.IsVisibleToClients = target.IsVisibleToClients || source.IsVisibleToClients;
        target.IsInherited = target.IsInherited || source.IsInherited;
        target.IsEncrypted = target.IsEncrypted || source.IsEncrypted;

        foreach (var property in source.Properties)
        {
            target.Properties[property.Key] = property.Value;
        }

        foreach (var provider in source.Providers)
        {
            if (!target.Providers.Contains(provider))
            {
                target.Providers.Add(provider);
            }
        }
    }
}

